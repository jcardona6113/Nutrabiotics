using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NutraBiotics.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NutraBiotics.Models;
using NutraBiotics.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NutraBiotics.ViewModels
{
    public class SearchInvoicesViewModel : INotifyPropertyChanged
    {

        #region Attributes
        ObservableCollection<InvoiceHeader> _invoiceheaders;
        List<InvoiceHeader> invoiceHeaders;
        Customer _customer;
        bool _isRefreshing;
        string _filter;
        Calendar _calendar;
        #endregion

        #region Services

        DataService dataService;
        NavigationService navigationService;
        DialogService dialogService;

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties

        public bool IsRefreshing
        {
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
            get
            {
                return _isRefreshing;
            }
        }

        public Calendar Calendar
        {
            set
            {
                if (_calendar != value)
                {
                    _calendar = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Calendar)));
                }
            }
            get
            {
                return _calendar;
            }
        }

        public Customer Customer
        {
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Customer)));
                }
            }
            get
            {
                return _customer;
            }
        }


        public ObservableCollection<InvoiceHeader> InvoiceHeaders
        {
            set
            {
                if (_invoiceheaders != value)
                {
                    _invoiceheaders = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(InvoiceHeaders)));
                }
            }
            get
            {
                return _invoiceheaders;
            }
        }

        #endregion

        #region Constructor

        public SearchInvoicesViewModel()
        {
            instance = this;
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            InvoiceHeaders = new ObservableCollection<InvoiceHeader>();


        }

        #endregion

        #region Singleton
        static SearchInvoicesViewModel instance;

        public static SearchInvoicesViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Methods


        #endregion

        #region Commands

        public ICommand SearchCustomerInvoiceCommand
        {
            get { return new RelayCommand(SearchCustomerInvoice); }
        }

        async void SearchCustomerInvoice()
        {
            var customers = dataService.Get<Customer>(false);

            if (customers == null || customers.Count == 0)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Debes descargar maestros.");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EjecutadoDesde = "SearchInvoicesViewModel";
            mainViewModel.SearchCustomer = new SearchCustomerViewModel(customers);
            await navigationService.Navigate("SearchCustomerPage");
        }

        public ICommand SearchCalendarCommand
        {
            get { return new RelayCommand(SearchCalendar); }
        }

        async void SearchCalendar()
        {
            try
            {
                var calendars = dataService.Get<Calendar>(false);

                if (calendars == null || calendars.Count == 0)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "Debes descargar los calendarios.");
                    return;
                }


                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.SearchCalendar = new SearchCalendarViewModel(calendars);
                await navigationService.Navigate("SearchCalendarPage");
            }
            catch (Exception ex)
            {
               await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        async void Search()
        {

            try
            {
                if (Customer == null || Calendar == null)
                {
                    await dialogService.ShowMessage("Validacion",
                        "Debes seleccionar un cliente y un calendario antes de ejecutar la busqueda.");
                    return;
                }

                var invoices = dataService
                .Get<InvoiceHeader>(true)
                .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                .ToList();
                InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoices);
            }


            catch (Exception ex)
            {

                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

    }
}


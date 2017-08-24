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
        Calendar _calendar;
        TimeSpan _diasvencido;
        bool _facturaconsaldo;
        decimal _totallineas;
        decimal _totalSaldo;
        bool _filtrofechas;
        bool isEnabled;
        DateTime _fechaInicial;
        DateTime _fechaFinal;


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

        public decimal TotalLineas
        {
            set
            {
                if (_totallineas != value)
                {
                    _totallineas = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLineas)));
                }
            }
            get
            {
                return _totallineas;
            }
        }

        public DateTime FechaInicial
        {
            set
            {
                if (_fechaInicial != value)
                {
                    _fechaInicial = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FechaInicial)));
                }
            }
            get
            {
                return _fechaInicial;
            }
        }

        public DateTime FechaFinal
        {
            set
            {
                if (_fechaFinal != value)
                {
                    _fechaFinal = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FechaFinal)));
                }
            }
            get
            {
                return _fechaFinal;
            }
        }

        public decimal TotalSaldo
        {
            set
            {
                if (_totalSaldo != value)
                {
                    _totalSaldo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalSaldo)));
                }
            }
            get
            {
                return _totalSaldo;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
            get
            {
                return isEnabled;
            }
        }

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

        public bool FacturaConSaldo
        {
            set
            {
                if (_facturaconsaldo != value)
                {
                    _facturaconsaldo = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(FacturaConSaldo)));
                }
            }
            get
            {
                return _facturaconsaldo;
            }
        }

        public bool FiltroFechas
        {
            set
            {
                if (_filtrofechas != value)
                {
                    _filtrofechas = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(FiltroFechas)));
                }
            }
            get
            {
                return _filtrofechas;
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
            IsEnabled = true;
            FechaInicial = DateTime.Now;
            FechaFinal = DateTime.Now;
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
                mainViewModel.EjecutadoDesde = "SearchInvoicesViewModel";
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
                IsEnabled = false;

                if (FacturaConSaldo)
                {
                    if (Customer == null || Calendar == null)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "Debes seleccionar un cliente y un periodo antes de ejecutar la busqueda.");
                        IsEnabled = true;
                        return;
                    }



                    var facturasconsaldo = dataService
                        .Get<InvoiceHeader>(true)
                        .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")) && s.InvoiceBal > 0)
                        .ToList();


                    if (facturasconsaldo == null || facturasconsaldo.Count == 0)

                    {
                        await dialogService.ShowMessage("Informacion", "El cliente, no posee facturas con saldo.");
                        return;
                    }

                    InvoiceHeaders = new ObservableCollection<InvoiceHeader>(facturasconsaldo);
                }
                    if (FiltroFechas)
                    {

                        if (Customer == null || FechaInicial == null || FechaFinal == null)
                        {
                            await dialogService.ShowMessage("Validacion",
                                "Debes seleccionar un cliente y un rango de fechas antes de ejecutar la busqueda.");
                            IsEnabled = true;
                            return;
                        }


                        var invoicesxfechas = dataService
                         .Get<InvoiceHeader>(true)
                         .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                         .ToList();

                        if (invoicesxfechas == null || invoicesxfechas.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no tiene facturas registradas en el rango seleccionado.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoicesxfechas);

                    TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                    TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                }

                if (FiltroFechas && FacturaConSaldo)
                {

                    if (Customer == null || FechaInicial == null || FechaFinal == null)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "Debes seleccionar un cliente y un rango de fechas antes de ejecutar la busqueda.");
                        IsEnabled = true;
                        return;
                    }


                    var invoicesxfechas = dataService
                     .Get<InvoiceHeader>(true)
                     .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")) && s.InvoiceBal > 0)
                     .ToList();

                    if (invoicesxfechas == null || invoicesxfechas.Count == 0)

                    {
                        await dialogService.ShowMessage("Informacion", "El cliente, no tiene facturas con saldo en el rango seleccionado.");
                        return;
                    }

                    InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoicesxfechas);

                    TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                    TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                }


                if (Customer == null || Calendar == null)
                {
                    await dialogService.ShowMessage("Validacion",
                        "Debes seleccionar un cliente y un periodo antes de ejecutar la busqueda.");
                    IsEnabled = true;
                    return;
                }

                var invoices = dataService
                 .Get<InvoiceHeader>(true)
                 .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                 .ToList();

                if (invoices == null || invoices.Count == 0)

                {
                    await dialogService.ShowMessage("Informacion", "El cliente, no tiene facturas registradas en el periodo seleccionado.");
                    return;
                }

                InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoices);
                TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
            }

            catch (Exception ex)
            {
                IsEnabled = true;
                await dialogService.ShowMessage("Error", ex.Message);
            }

            IsEnabled = true;
        }

        #endregion

    }
}


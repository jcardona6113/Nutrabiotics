namespace NutraBiotics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NutraBiotics.Models;
    using NutraBiotics.Services;
    using NutraBiotics.Data;
    using NutraBiotics.Helpers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class SearchCashHeaderViewModel : INotifyPropertyChanged
    {
        #region Attributes

        ObservableCollection<CashHeader> _cashHeaders;
        List<CashHeader> cashHeader;
        string _filter;
        bool _isRefreshing;
        DialogService dialogService;
        NavigationService navigationService;
        Customer _customer;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        DataService dataService; 

        #endregion

        #region Properties

        public ObservableCollection<CashHeader> CashHeaders
        {
            set
            {
                if (_cashHeaders != value)
                {
                    _cashHeaders = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CashHeaders)));
                }
            }
            get
            {
                return _cashHeaders;
            }
        }

        public Customer Customer
        {
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Customer)));
                }
            }
            get
            {
                return _customer;
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

        //public string Filter
        //{
        //    set
        //    {
        //        if (_filter != value)
        //        {
        //            _filter = value;
        //            if (string.IsNullOrEmpty(_filter))
        //            {
        //                ReloadCashHeaders();
        //            }
        //            else
        //            {
        //                Search();
        //            }

        //            PropertyChanged?.Invoke(
        //                this,
        //                new PropertyChangedEventArgs(nameof(Filter)));
        //        }
        //    }
        //    get
        //    {
        //        return _filter;
        //    }

        //}


        #endregion

        #region Singleton
        static SearchCashHeaderViewModel instance;

        public static SearchCashHeaderViewModel GetInstance()
        {
            return instance;
        }

        #endregion

        #region Constructor

        public SearchCashHeaderViewModel(List<CashHeader> cashheaderlist)
        {
            instance = this;
            this.cashHeader = cashheaderlist;
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            CashHeaders = new ObservableCollection<CashHeader>();
            // CargarPagos();
        } 
       
        #endregion

        #region Methods

        //void ReloadCashHeaders()
        //{
        //    CashHeaders = new ObservableCollection<Grouping<string, CashHeader>>(
        //        cashHeader
        //        .OrderBy(s => s.Names)
        //        .GroupBy(s => s.Names[0].ToString(), s => s)
        //        .Select(g => new Grouping<string, CashHeader>(g.Key, g)));
        //}


      public async void CargarPagos()
        {
            try
            {
                var pagos = dataService
                .Get<CashHeader>(true)
                .Where(s => s.CustId == Customer.CustId)
                .ToList();
                CashHeaders = new ObservableCollection<CashHeader>(pagos);
            }
            catch (Exception ex)
            {

                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

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
            mainViewModel.EjecutadoDesde = "SearchCashHeaderPage";
            mainViewModel.SearchCustomer = new SearchCustomerViewModel(customers);
            await navigationService.Navigate("SearchCustomerPage");
        }


        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

       async void Search()
        {
            try
            {
                var pagos = dataService
                .Get<CashHeader>(true)
                .Where(s => s.CustId == Customer.CustId)
                .ToList();
                CashHeaders = new ObservableCollection<CashHeader>(pagos);
            }
            catch (Exception ex)
            {

                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

    }
}

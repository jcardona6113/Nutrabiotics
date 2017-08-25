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
        decimal _totallineas;
        Calendar _calendar;
        bool _filtrofechas;
        bool isEnabled;
        DateTime _fechaInicial;
        DateTime _fechaFinal;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        DataService dataService;

        #endregion

        #region Properties

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

        public SearchCashHeaderViewModel()
        {
            instance = this;
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            CashHeaders = new ObservableCollection<CashHeader>();
            IsEnabled = false;
            FechaInicial = DateTime.Now;
            FechaFinal = DateTime.Now;

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


        public ICommand SearchCustomerCommand
        {
            get { return new RelayCommand(SearchCustomer); }
        }

        async void SearchCustomer()
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
                mainViewModel.EjecutadoDesde = "SearchCashHeaderPage";
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
                CashHeaders = null;

                if (Calendar != null && FiltroFechas == false)
                {

                    if (Customer == null)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "Debes seleccionar un cliente antes de ejecutar la busqueda.");
                        return;
                    }


                    var pagos = dataService
                        .Get<CashHeader>(true)
                        .Where(s => s.CustId == Customer.CustId && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                        .ToList();


                    if (pagos == null || pagos.Count == 0)

                    {
                        await dialogService.ShowMessage("Informacion", "El cliente, no registra pagos en el periodo seleccionado.");
                        return;
                    }

                    CashHeaders = new ObservableCollection<CashHeader>(pagos);

                    TotalLineas = CashHeaders.Sum(god => god.TranAmt);
                }

                if (Calendar == null && FiltroFechas == true)
                {

                    if (Customer == null)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "Debes seleccionar un cliente antes de ejecutar la busqueda.");
                        return;
                    }


                    var pagos = dataService
                        .Get<CashHeader>(true)
                        .Where(s => s.CustId == Customer.CustId && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(FechaInicial.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(FechaFinal.ToString("yyyy/MM/dd")))
                        .ToList();


                    if (pagos == null || pagos.Count == 0)

                    {
                        await dialogService.ShowMessage("Informacion", "El cliente, no registra pagos en el rango de fechas seleccionado.");
                        return;
                    }

                    CashHeaders = new ObservableCollection<CashHeader>(pagos);

                    TotalLineas = CashHeaders.Sum(god => god.TranAmt);
                }


            }

            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        
        #endregion
    }
}

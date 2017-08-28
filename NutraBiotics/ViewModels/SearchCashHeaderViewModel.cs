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
        bool _hayFiltro;
        bool _filtroPeriodo;

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


        public bool FiltroPeriodo
        {
            set
            {
                if (_filtroPeriodo != value)
                {
                    _filtroPeriodo = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(FiltroPeriodo)));
                }
            }
            get
            {
                return _filtroPeriodo;
            }
        }


        public bool HayFiltro
        {
            set
            {
                if (_hayFiltro != value)
                {
                    _hayFiltro = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(HayFiltro)));
                }
            }
            get
            {
                return _hayFiltro;
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


        #endregion

        #region Singleton
        static SearchCashHeaderViewModel instance;

        public static SearchCashHeaderViewModel GetInstance()
        {
            if (instance == null)
            {
                return new SearchCashHeaderViewModel();
            }

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
            FechaInicial = DateTime.Now;
            FechaFinal = DateTime.Now;

            // CargarPagos();
        } 
       
        #endregion

        #region Methods

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
                        "Debes descargar periodos.");
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

       public async void Search(){
            try
            {
                if (Customer == null)
                {
                    await dialogService.ShowMessage("Validacion",
                        "Debes seleccionar un cliente antes de ejecutar la busqueda.");
                    return;
                }

                CashHeaders.Clear();
                TotalLineas = 0;

                if (FiltroFechas || FiltroPeriodo) { HayFiltro = true; }

                if (HayFiltro)
                {

                    if (FiltroFechas && FiltroPeriodo)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "No es posible filtrar por rango de fechas y periodo a la vez.");
                        return;
                    }


                    //Si se filtro x rango de fechas...

                    if (FiltroFechas == true && FiltroPeriodo == false)
                    {
                        var pagosxfecha = dataService
                            .Get<CashHeader>(true)
                            .Where(s => s.CustId == Customer.CustId && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(FechaInicial.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(FechaFinal.ToString("yyyy/MM/dd")))
                            .ToList();

                        if (pagosxfecha == null || pagosxfecha.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra pagos en el rango de fechas seleccionado.");
                            return;
                        }

                        CashHeaders = new ObservableCollection<CashHeader>(pagosxfecha);
                        TotalLineas = CashHeaders.Sum(god => god.TranAmt);
                    }


                    //Si se filtro x periodo...

                    if (FiltroPeriodo == true && FiltroFechas == false)
                    {

                        if (Calendar == null)
                        {
                            await dialogService.ShowMessage("Informacion", "Debes seleccionar un periodo.");
                            return;
                        }


                        var pagosxperiodo = dataService
                        .Get<CashHeader>(true)
                        .Where(s => s.CustId == Customer.CustId && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.TranDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                        .ToList();


                        if (pagosxperiodo == null || pagosxperiodo.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra pagos en el periodo seleccionado.");
                            return;
                        }

                        CashHeaders = new ObservableCollection<CashHeader>(pagosxperiodo);

                        TotalLineas = CashHeaders.Sum(god => god.TranAmt);
                    }
                }

                var pagos = dataService
                       .Get<CashHeader>(true)
                       .Where(s => s.CustId == Customer.CustId)
                       .ToList();

                if (pagos == null || pagos.Count == 0)

                {
                    await dialogService.ShowMessage("Informacion", "El cliente, no registra pagos.");
                    return;
                }

                CashHeaders = new ObservableCollection<CashHeader>(pagos);
                TotalLineas = CashHeaders.Sum(god => god.TranAmt);

            }

            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion
    }
}

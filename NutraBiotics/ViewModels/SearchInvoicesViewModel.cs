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
        decimal _montomenossaldo;
        bool _filtrofechas;
        bool isEnabled;
        DateTime _fechaInicial;
        DateTime _fechaFinal;
        bool _filtroFechasIsEnabled;
        bool _hayFiltro;
        bool _filtroPeriodo;


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

        public bool FiltroFechasIsEnabled
        {
            set
            {
                if (_filtroFechasIsEnabled != value)
                {
                    _filtroFechasIsEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FiltroFechasIsEnabled)));
                }
            }
            get
            {
                return _filtroFechasIsEnabled;
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


        public decimal MontoMenosSaldo
        {
            set
            {
                if (_montomenossaldo != value)
                {
                    _montomenossaldo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MontoMenosSaldo)));
                }
            }
            get
            {
                return _montomenossaldo;
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
            FechaInicial = DateTime.Now;
            FechaFinal = DateTime.Now;
            InvoiceHeaders = new ObservableCollection<InvoiceHeader>();
        }

        #endregion

        #region Singleton
        static SearchInvoicesViewModel instance;

        public static SearchInvoicesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new SearchInvoicesViewModel();
            }

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
                        "Debes descargar periodos.");
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

        public async void Search()
        {
            try
            {
                if (Customer == null)
                {
                    await dialogService.ShowMessage("Validacion",
                        "Debes seleccionar un cliente antes de ejecutar la busqueda.");
                    return;
                }

                InvoiceHeaders.Clear();
                TotalLineas = 0;

                if (FiltroFechas || FiltroPeriodo || FacturaConSaldo) { HayFiltro = true; }

                if (HayFiltro)
                {

                    if (FiltroFechas && FiltroPeriodo)
                    {
                        await dialogService.ShowMessage("Validacion",
                            "No es posible filtrar por rango de fechas y periodo a la vez.");
                        return;
                    }

                    //filtro x fact saldo
                    if (FacturaConSaldo == true && FiltroPeriodo == false && FiltroFechas == false)
                    {

                        var facturasconsaldo = dataService
                            .Get<InvoiceHeader>(true)
                            .Where(s => s.CustNum == Customer.CustNum && s.InvoiceBal > 0)
                            .ToList();


                        if (facturasconsaldo == null || facturasconsaldo.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas con saldo.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(facturasconsaldo);
                        TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                        TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                        MontoMenosSaldo = (TotalLineas - TotalSaldo);
                    }

                    //filtro x fechas
                    if (FiltroFechas == true && FiltroPeriodo == false && FacturaConSaldo == false)
                    {

                        var invoicesxfechas = dataService
                         .Get<InvoiceHeader>(true)
                         .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(FechaInicial.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(FechaFinal.ToString("yyyy/MM/dd")))
                         .ToList();

                        if (invoicesxfechas == null || invoicesxfechas.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas en el rango de fechas seleccionado.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoicesxfechas);
                        TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                        TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                        MontoMenosSaldo = (TotalLineas - TotalSaldo);
                    }

                    //filtro x fechas y fact. con saldo
                    if (FiltroFechas == true && FacturaConSaldo == true && FiltroPeriodo == false)
                    {
                        var invoicesxfechas = dataService
                         .Get<InvoiceHeader>(true)
                         .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(FechaInicial.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(FechaFinal.ToString("yyyy/MM/dd")) && s.InvoiceBal > 0)
                         .ToList();

                        if (invoicesxfechas == null || invoicesxfechas.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas con saldo en el rango de fechas seleccionado.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(invoicesxfechas);
                        TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                        TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                        MontoMenosSaldo = (TotalLineas - TotalSaldo);
                    }

                    //filtro x periodo
                    if (FiltroPeriodo == true && FiltroFechas == false && FacturaConSaldo == false)
                    {

                        if (Calendar == null)
                        {
                            await dialogService.ShowMessage("Informacion", "Debes seleccionar un periodo.");
                            return;
                        }


                        var facturasxperiodo = dataService
                         .Get<InvoiceHeader>(true)
                         .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                         .ToList();

                        if (facturasxperiodo == null || facturasxperiodo.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas en el periodo seleccionado.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(facturasxperiodo);
                        TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                        TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                        MontoMenosSaldo = (TotalLineas - TotalSaldo);
                    }

                    //filtro x periodo y fact. con saldo
                    if (FiltroPeriodo == true && FacturaConSaldo == true && FiltroFechas == false)
                    {
                        if (Calendar == null)
                        {
                            await dialogService.ShowMessage("Informacion", "Debes seleccionar un periodo.");
                            return;
                        }


                        var facturasxpreiodo = dataService
                         .Get<InvoiceHeader>(true)
                         .Where(s => s.CustNum == Customer.CustNum && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Calendar.StartDate.ToString("yyyy/MM/dd")) && Convert.ToDateTime(s.InvoiceDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Calendar.EndDate.ToString("yyyy/MM/dd")))
                         .ToList();

                        if (facturasxpreiodo == null || facturasxpreiodo.Count == 0)

                        {
                            await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas en el periodo seleccionado.");
                            return;
                        }

                        InvoiceHeaders = new ObservableCollection<InvoiceHeader>(facturasxpreiodo);
                        TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                        TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                        MontoMenosSaldo = (TotalLineas - TotalSaldo);
                    }

                }

                else

                {

                    var facturas = dataService
                           .Get<InvoiceHeader>(true)
                           .Where(s => s.CustNum == Customer.CustNum)
                           .ToList();

                    if (facturas == null || facturas.Count == 0)

                    {
                        await dialogService.ShowMessage("Informacion", "El cliente, no registra facturas.");
                        return;
                    }

                    InvoiceHeaders = new ObservableCollection<InvoiceHeader>(facturas);
                    TotalLineas = InvoiceHeaders.Sum(god => god.InvoiceAmt);
                    TotalSaldo = InvoiceHeaders.Sum(god => god.InvoiceBal);
                    MontoMenosSaldo = (TotalLineas - TotalSaldo);
                }

                await navigationService.Navigate("InvoicesListPage");

            }

            catch (Exception ex)
            {

                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

    }
}


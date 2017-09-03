namespace NutraBiotics.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Xamarin.Forms;
    using Data;
    using System.Linq;
    using System;

    public class DownloadViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        DialogService dialogService;
        ApiService apiService;
        NetService netService;
        DataService dataService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        MainViewModel main = MainViewModel.GetInstance();
        double _progress;
        bool _isRunning;
        bool _isEnabled;
        string _message;
        bool _descargarClientes;
        bool _descargartodos;
        bool _descargarCartera;

        List<Customer> customers;
        List<ShipTo> shipTos;
        // List<Contact> contacts;
        List<Part> parts;
        List<PriceList> pricelist;
        List<PriceListPart> pricelistpart;
        List<CustomerPriceList> customerpricelist;
        List<InvoiceHeader> invoiceheaders;
        List<InvoiceDetail> invoicedetails;
        List<CashHeader> cashheaders;
        List<Territory> territories;
        List<Country> countries;
        List<PersonContact> personcontacts;
        List<SalesRep> salesrepresents;
        List<Calendar> calendars;


        #endregion

        #region Properties

        public bool DescargarTodos
        {
            set
            {
                if (_descargartodos != value)
                {
                    _descargartodos = value;
                    DescargarCartera = value;
                    DescargarClientes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DescargarTodos)));
                }
            }
            get
            {
                return _descargartodos;
            }
        }

        public bool DescargarClientes
        {
            set
            {
                if (_descargarClientes != value)
                {
                    _descargarClientes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DescargarClientes)));
                }
            }
            get
            {
                return _descargarClientes;
            }
        }

        public bool DescargarCartera
        {
            set
            {
                if (_descargarCartera != value)
                {
                    _descargarCartera = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DescargarCartera)));
                }
            }
            get
            {
                return _descargarCartera;
            }
        }





        public string Message
        {
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
            get
            {
                return _message;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
            get
            {
                return _isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
            get
            {
                return _isEnabled;
            }
        }

        public double Progress
        {
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
                }
            }
            get
            {
                return _progress;
            }
        }
        #endregion

        #region Constructors
        public DownloadViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();
            netService = new NetService();
            dataService = new DataService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand DownloadCommand
        {
            get { return new RelayCommand(Download); }
        }

        async void Download()
        {
            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de iniciar una nueva descarga?");
            if (!answer)
            {
                return;
            }

            var connection = await netService.CheckConnectivity();
            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            await BeginDownload();
        }
        #endregion

        #region Methods
        async Task BeginDownload()
        {
            try
            {
                IsRunning = true;
                IsEnabled = false;

                Progress = 0;
                var url = Application.Current.Resources["URLAPI"].ToString();

                if (DescargarClientes)
                {

                    salesrepresents = await DownloadMaster<SalesRep>(url, "/api/SalesReps/"+main.User.VendorId);

                    if (salesrepresents == null)
                    {
                        DeleteAll<SalesRep>();
                    }

                    var query = salesrepresents.GroupBy(x => x.VendorId)
                                    .Select(g => g.First());

                    foreach (var item in query)
                    {

                        List<Customer> customerstemp;
                        Message = "Descargando clientes...";
                        customerstemp = await DownloadMaster<Customer>(url, "/api/Customers/" + item.VendorId);
                        customers = customerstemp.Where(c => c.CustId != "22" && c.CustId != "23" && c.CustId != "24" && c.CustId != "25").ToList();

                        if (customers != null && customers.Count > 0)
 
                        {
                            DeleteAndInsert(customers);
                            await Task.Delay(100);
                        }


                        List<ShipTo> Shiptostemp;
                        Message = "Descargando sucursales...";
                        Shiptostemp = await DownloadMaster<ShipTo>(url, "/api/ShipToes/" + item.VendorId);

                        if (Shiptostemp == null)
                        {
                            DeleteAll<ShipTo>();
                        }


                        var shiptoesjoin = from s in Shiptostemp
                                           join c in customers on s.CustNum equals c.CustNum
                                           where c.CustomerId == s.CustomerId && c.CustId != "22" && c.CustId != "23" && c.CustId != "24" && c.CustId != "25"
                                           select s;

                        shipTos = shiptoesjoin.ToList();

                        if (shipTos != null && shipTos.Count > 0)
                        {
                            DeleteAndInsert(shipTos);
                            await Task.Delay(100);
                        }



                    }

                    Message = "Descargando territorios...";
                    territories = await DownloadMaster<Territory>(url, "/api/Territories/");

                    if (territories == null)
                    {
                        DeleteAll<Territory>();
                    }


                    if (territories != null && territories.Count > 0)
                    {
                        DeleteAndInsert(territories);
                        await Task.Delay(100);
                    }

                    Message = "Descargando paises...";
                    countries = await DownloadMaster<Country>(url, "/api/Countries/");

                    if (countries == null)
                    {
                        DeleteAll<Country>();
                    }


                    if (countries != null && countries.Count > 0)
                    {
                        DeleteAndInsert(countries);
                        await Task.Delay(100);
                    }


                    Message = "Descargando Partes...";
                    parts = await DownloadMaster<Part>(url, "/api/Parts");


                    if (parts == null)
                    {
                        DeleteAll<Part>();
                    }


                    if (parts != null && parts.Count > 0)
                    {
                        DeleteAndInsert(parts);
                        await Task.Delay(100);
                    }


                    Message = "Descargando lista de precios...";
                    pricelist = await DownloadMaster<PriceList>(url, "/api/PriceLists");

                    if (pricelist == null)
                    {
                        DeleteAll<PriceList>();
                    }


                    if (pricelist != null && pricelist.Count > 0)
                    {
                        DeleteAndInsert(pricelist);
                        await Task.Delay(100);
                    }


                    Message = "Descargando lista de precios x parte...";
                    pricelistpart = await DownloadMaster<PriceListPart>(url, "/api/PriceListParts");

                    if (pricelistpart == null)
                    {
                        DeleteAll<PriceListPart>();
                    }


                    if (pricelistpart != null && pricelistpart.Count > 0)
                    {
                        DeleteAndInsert(pricelistpart);
                        await Task.Delay(100);
                    }


                    List<CustomerPriceList> customerpricelistTemp;
                    Message = "Descargando lista de precios x cliente...";
                    customerpricelistTemp = await DownloadMaster<CustomerPriceList>(url, "/api/CustomerPriceLists");


                    if (customerpricelistTemp == null)
                    {
                        DeleteAll<CustomerPriceList>();
                    }



                    var customerpricelistjoin = from cp in customerpricelistTemp
                                                join c in customers on cp.CustomerId equals c.CustomerId
                                                where c.VendorId == main.User.VendorId
                                                select cp;

                    customerpricelist = customerpricelistjoin.ToList();

                    if (customerpricelist != null && customerpricelist.Count > 0)
                    {
                        DeleteAndInsert(customerpricelist);
                        await Task.Delay(100);
                    }
                }


                if (DescargarCartera)
                {
                    Message = "Descargando facturas...";
                    invoiceheaders = await DownloadMaster<InvoiceHeader>(url, "/api/InvoiceHeaders/" + main.User.VendorId);

                    if (invoiceheaders == null)
                    {
                        DeleteAll<InvoiceHeader>();
                    }


                    if (invoiceheaders != null && invoiceheaders.Count > 0)
                    {
                        DeleteAndInsert(invoiceheaders);
                        await Task.Delay(100);
                    }



                    Message = "Descargando periodos...";
                    calendars = await DownloadMaster<Calendar>(url, "/api/Calendars/");


                    if (calendars == null)
                    {
                        DeleteAll<Calendar>();
                    }

                    if (calendars != null && calendars.Count > 0)
                    {
                        DeleteAndInsert(calendars);
                        await Task.Delay(100);
                    }


                    invoicedetails = await DownloadMaster<InvoiceDetail>(url, "/api/InvoiceDetails/" + main.User.VendorId);


                    if (invoicedetails == null)
                    {
                        DeleteAll<InvoiceDetail>();
                    }

                    if (invoicedetails != null && invoicedetails.Count > 0)
                    {
                        DeleteAndInsert(invoicedetails);
                        await Task.Delay(100);
                    }


                    Message = "Descargando pagos...";
                    cashheaders = await DownloadMaster<CashHeader>(url, "/api/CashHeaders/" + main.User.VendorId);

                    if (cashheaders == null)
                    {
                        DeleteAll<CashHeader>();
                    }

                    if (cashheaders != null && cashheaders.Count > 0)
                    {
                        DeleteAndInsert(cashheaders);
                        await Task.Delay(100);
                    }
                }
                Message = "Proceso finalizado...";
                Progress = 1;

                IsRunning = false;
                IsEnabled = true;

                await dialogService.ShowMessage(
                    "Confirmación",
                    "Proceso finalizado con éxito.");
                await navigationService.Back();
            }
            catch (System.Exception ex)
            {

                Message = ex.Message; ;
            }
        }

        void DeleteAndInsert<T>(List<T> list) where T : class
        {
            using (var da = new DataAccess())
            {
                var oldRecords = da.GetList<T>(false);
                foreach (var oldRecord in oldRecords)
                {
                    da.Delete(oldRecord);
                }

                foreach (var record in list)
                {
                    da.Insert(record);
                }
            }
        }

        public bool DeleteAll<T>() where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>(false);
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete(oldRecord);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

            async Task<List<T>> DownloadMaster<T>(string url, string controller) where T : class
        {
            var response = await apiService.GetList<T>(url, controller);
            if (!response.IsSuccess)
            {
                Message = response.Message;
            }

            return (List<T>)response.Result;
        }
        #endregion
    }
}
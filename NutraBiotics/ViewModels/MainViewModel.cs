namespace NutraBiotics.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Xamarin.Forms;
    using System.ComponentModel;

    public class MainViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes

        // Es para definir comportamiento luego de seleccionar cliente en formulario de consulta.
        public string EjecutadoDesde;

        #endregion

        #region Services
        NavigationService navigationService;
        DialogService dialogService;
        DataService dataService;
        ApiService apiService;
        NetService netService;
        #endregion

        #region Properties


        public User User
        {
            get;
            set;
        }

        public LoginViewModel Login
        {
            get;
            set;
        }

        public DownloadViewModel Download
        {
            get;
            set;
        }

        public NewOrderViewModel NewOrder
        {
            get;
            set;
        }

        public SearchCustomerViewModel SearchCustomer
        {
            get;
            set;
        }

        public SearchShipToViewModel SearchShipTo
        {
            get;
            set;
        }

        public SearchContactViewModel SearchContact
        {
            get;
            set;
        }

        public SearchPriceListPartViewModel SearchPriceListPart
        {
            get;
            set;
        }

        public CustomersViewModel CustomersVM
        {
            get;
            set;
        }

        public EditCustomerViewModel EditCustomer
        {
            get;
            set;
        }

        public EditOrderViewModel EditOrder
        {
            get;
            set;
        }

        public SearchInvoicesViewModel InvoicesHeaders
        {
            get;
            set;
        }

        public PersonContactsViewModel PersonContacts
        {
            get;
            set;
        }

        public EditShipToViewModel EditShipTo
        {
            get;
            set;
        }

        public EditContactViewModel EditContact
        {
            get;
            set;
        }

        public NewShipToViewModel NewShipTo
        {
            get;
            set;
        }

        public EditOrderDetailsViewModel EditOrderDetails
        {
            get;
            set;
        }


        public SearchCalendarViewModel SearchCalendar
        {
            get;
            set;
        }

        public NewContactViewModel NewContact
        {
            get;
            set;
        }

        public OrdersViewModel Orders
        {
            get;
            set;
        }

        public SearchCashHeaderViewModel SearchCashHeader
        {
            get;
            set;
        }


        public InvoiceDetailViewModel InvoiceDetail
        {
            get;
            set;
        }

        public UploadViewModel Upload
        {
            get;
            set;
        }

        public SearchInvoicesViewModel SearchInvoices
        {
            get;
            set;
        }

        public NewPersonContactViewModel NewPersonContact
        {
            get;
            set;
        }

        public ObservableCollection<MenuItemViewModel> Menu
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            navigationService = new NavigationService();
            dialogService = new DialogService();
            dataService = new DataService();
            apiService = new ApiService();
            netService = new NetService();

            Login = new LoginViewModel();

            Menu = new ObservableCollection<MenuItemViewModel>();
            LoadMenu();
        }
        #endregion

        #region Singleton
        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods

        void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_cloud_download.png",
                PageName = "DownloadPage",
                Title = "Descargar Maestros",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_people.png",
                PageName = "CustomersPage",
                Title = "Clientes",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_monetization_on.png",
                PageName = "TabPursePage",
                Title = "Cartera",
            });


            Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "",
                Title = "",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "",
                Title = "",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "",
                Title = "",
            });



            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
                Title = "Cerrar Sesión",
            });
        }

        void ClearOrderFields()
        {
            NewOrder.Customer = null;
            NewOrder.ShipTo = null;
            NewOrder.PriceListId = 0;
            NewOrder.Reference= "0";
            NewOrder.Observations = null;
            NewOrder.NeedBy = DateTime.Today;
            NewOrder.TotalLineas = 0;
            NewOrder.Part = null;
            NewOrder.PartNum = null;
            NewOrder.Observations = null;
            NewOrder.GridOrderDetails.Clear();
        }

        async void GoSaveOrder()
        {
            try
            {

                var orderHeader = new OrderHeader
                {
                    UserId = User.UserId,
                    VendorId = User.VendorId,
                    ShipToNum = NewOrder.ShipTo.ShipToNum,
                    CustomerId = NewOrder.Customer.CustomerId,
                    CustId = NewOrder.Customer.CustId,
                    ContactId = 0,
                    CreditHold = NewOrder.Customer.CreditHold,
                    Date = DateTime.Now,
                    Platform = "Movil",
                    TermsCode = NewOrder.Customer.TermsCode,
                    ShipToId = NewOrder.ShipTo.ShipToId,
                    SalesCategory = string.Empty,
                    InvoiceNum = 0,
                    Facturado = false,
                    Observations = NewOrder.Observations,
                    IsSync = false,
                    Total = NewOrder.TotalLineas,
                    SincronizadoEpicor = false,
                    NeedByDate = NewOrder.NeedBy,
                    RowMod = NewOrder.RowMod,
                };

                if (orderHeader.Observations == null) {orderHeader.Observations = String.Empty;}

                dataService.Insert(orderHeader);
                int i = 0;
                foreach (var detail in NewOrder.GridOrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderLine = ++i,
                        OrderQty = detail.Quantity,
                        PartId = detail.PartId,
                        PriceListPartId = detail.PriceListPartId,
                        Reference = detail.Reference,
                        PartNum = detail.PartNum,
                        SalesOrderHeaderId = orderHeader.SalesOrderHeaderId,
                        UnitPrice = detail.BasePrice,
                    };
                    dataService.Insert(orderDetail);
                }
                ClearOrderFields();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        async void GoUpdateOrder()
        {
            try
            {
                var orderHeader = new OrderHeader
                {
                    SalesOrderHeaderId = NewOrder.SalesOrderHeaderId,
                    OrderNum = NewOrder.OrderNum,
                    SalesOrderHeaderInterId = NewOrder.SalesOrderHeaderInterId,
                    UserId = User.UserId,
                    VendorId = User.VendorId,
                    ContactId = 0,
                    ConNum = 0,
                    InvoiceNum = 0,
                    Facturado = false,
                    ShipToNum = NewOrder.ShipTo.ShipToNum,
                    CustomerId = NewOrder.Customer.CustomerId,
                    CustId = NewOrder.Customer.CustId,
                    CreditHold = NewOrder.Customer.CreditHold,
                    Date = NewOrder.Date,
                    Platform = "Movil",
                    TermsCode = NewOrder.Customer.TermsCode,
                    ShipToId = NewOrder.ShipTo.ShipToId,
                    SalesCategory = string.Empty,
                    Observations = NewOrder.Observations,
                    IsSync = false,
                    Total = NewOrder.TotalLineas,
                    SincronizadoEpicor = NewOrder.SincronizadoEpicor,
                    NeedByDate = NewOrder.NeedBy,
                    RowMod = NewOrder.RowMod,
                };

                dataService.InsertOrUpdate(orderHeader);
                int i = 0;
                foreach (var detail in NewOrder.GridOrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderLine = ++i,
                        SalesOrderDetaliId = detail.SalesOrderDetaliId,
                        SalesOrderHeaderId = orderHeader.SalesOrderHeaderId,
                        PriceListPartId = detail.PriceListPartId,
                        PartId = detail.PartId,
                        OrderQty = detail.Quantity,
                        Reference = detail.Reference,
                        PartNum = detail.PartNum,
                        UnitPrice = detail.BasePrice,
                    };
                    dataService.InsertOrUpdate(orderDetail);
                }

                ClearOrderFields();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        async void GoDeleteOrder()
        {

            var orders = dataService.Get<OrderHeader>(true).ToList();

            try
            {
                var orderHeader = new OrderHeader
                {
                    SalesOrderHeaderId = NewOrder.SalesOrderHeaderId,
                    OrderNum = NewOrder.OrderNum,
                    SalesOrderHeaderInterId = NewOrder.SalesOrderHeaderInterId,
                    UserId = User.UserId,
                    VendorId = User.VendorId,
                    ContactId = 0,
                    ConNum = 0,
                    InvoiceNum = 0,
                    Facturado = false,
                    Platform = "Movil",
                    ShipToNum = NewOrder.ShipTo.ShipToNum,
                    CustomerId = NewOrder.Customer.CustomerId,
                    CustId = NewOrder.Customer.CustId,
                    CreditHold = NewOrder.Customer.CreditHold,
                    Date = NewOrder.Date,
                    TermsCode = NewOrder.Customer.TermsCode,
                    ShipToId = NewOrder.ShipTo.ShipToId,
                    SalesCategory = string.Empty,
                    Observations = NewOrder.Observations,
                    IsSync = false,
                    Total = NewOrder.TotalLineas,
                    SincronizadoEpicor = NewOrder.SincronizadoEpicor,
                    NeedByDate = NewOrder.NeedBy,
                    RowMod = NewOrder.RowMod,
                };

                dataService.InsertOrUpdate(orderHeader);
                int i = 0;
                foreach (var detail in NewOrder.GridOrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderLine = ++i,
                        SalesOrderDetaliId = detail.SalesOrderDetaliId,
                        OrderQty = detail.Quantity,
                        PartId = detail.PartId,
                        PriceListPartId = detail.PriceListPartId,
                        Reference = detail.Reference,
                        PartNum = detail.PartNum,
                        SalesOrderHeaderId = orderHeader.SalesOrderHeaderId,
                        UnitPrice = detail.BasePrice,
                    };

                    dataService.InsertOrUpdate(orderDetail);
                }

                ClearOrderFields();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

        #region Commands
        public ICommand SyncOrdersCommand
        {
            get { return new RelayCommand(SyncOrders); }
        }

        async void SyncOrders()
        {
            var ordersPendingToSync = dataService
                .Get<OrderHeader>(true)
                .Where(o => !o.IsSync)
                .ToList();

            if (ordersPendingToSync.Count == 0)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "No tienes ordenes pendientes por sincronizar");
                return;
            }

            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de sincronizar las órdenes pendientes?");

            if (!answer)
            {
                return;
            }

            Orders.IsRefreshing = true;
            var url = Application.Current.Resources["URLAPI"].ToString();
            var controller = "/api/OrderHeaders";
            var response = await apiService.SyncOrder(
                url,
                controller,
                ordersPendingToSync);

            Orders.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            Orders.RefreshOrders();

            await dialogService.ShowMessage(
                "Confirmación",
                response.Message);
        }

        public ICommand RefreshOrdersNumCommand
        {
            get { return new RelayCommand(RefreshOrdersNum); }
        }

        async void RefreshOrdersNum()
        {
            var ordersPendingToRefresh = dataService
                .Get<OrderHeader>(true)
                .ToList();

            if (ordersPendingToRefresh.Count == 0)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "No tienes ordenes pendientes por actualizar");
                return;
            }


            Orders.IsRefreshing = true;
            var url = Application.Current.Resources["URLAPI"].ToString();
            var controller = "/api/OrderHeaders";
            var response = await apiService.RefreshOrders(
                url,
                controller,
                ordersPendingToRefresh);

            Orders.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            Orders.RefreshOrders();
            await dialogService.ShowMessage(
                "Confirmación",
                "Órdenes actualizadas Ok");
        }

        public ICommand SaveOrderCommand
        {
            get { return new RelayCommand(SaveOrder); }
        }

        public async void SaveOrder()
        {
            if (NewOrder.Customer == null)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Debes de seleccionar un cliente.");
                return;
            }

            if (NewOrder.ShipTo == null)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Debes de seleccionar una sucursal.");
                return;
            }

            if (NewOrder.GridOrderDetails.Count == 0)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Debes adicionar detalle.");
                return;
            }

            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de guardar la orden?");

            if (!answer)
            {
                return;
            }

            if (NewOrder.RowMod == "C")
            {
                GoSaveOrder();
                ClearOrderFields();

                await dialogService.ShowMessage(
                    "Confirmación",
                    "Orden guardada localmente OK");
            }

            else if (NewOrder.RowMod == "U")
            {
                GoUpdateOrder();
                ClearOrderFields();

                await dialogService.ShowMessage(
                    "Confirmación",
                    "Orden actualizada localmente OK");
            }
        }

        public ICommand ExecDeleteOrderCommand
        {
            get { return new RelayCommand(ExecDeleteOrder); }
        }

        public async void ExecDeleteOrder()
        {
            GoDeleteOrder();

            await dialogService.ShowMessage(
                "Confirmación",
                "Se elimino la orden");
        }

        public ICommand DeleteOrderCommand
        {
            get { return new RelayCommand(DeleteOrder); }
        }

        async void DeleteOrder()
        {
            if (NewOrder.GridOrderDetails.Count == 0)
            {
                return;
            }

            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de borrar lo que lleva de la orden?");

            if (answer)
            {
                NewOrder.TotalLineas = 0;
                NewOrder.GridOrderDetails.Clear();
            }
        }

        public ICommand NewOrderCommand
        {
            get { return new RelayCommand(GotoNewOrder); }
        }

        async void GotoNewOrder()
        {
            NewOrder = new NewOrderViewModel();
            NewOrder.RowMod = "C";
            await navigationService.Navigate("NewOrderTab");
        }

        public ICommand NewPersonContactCommand
        {
            get { return new RelayCommand(ExecNewPersonContact); }
        }

        async void ExecNewPersonContact()
        {
            NewPersonContact = new NewPersonContactViewModel();
            NewPersonContact.LoadCountries();
            await navigationService.Navigate("NewPersonContactPage");
        }

        public ICommand SaveShipToCommand
        {
            get { return new RelayCommand(SaveShipTo); }
        }

        async void SaveShipTo()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Guardar sucursal?");

                var connection = await netService.CheckConnectivity();
                if (!connection.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", connection.Message);
                    return;
                }

                if (string.IsNullOrEmpty(NewShipTo.ShipToNum))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar un ShipToNum.");
                    return;
                }

                if (string.IsNullOrEmpty(NewShipTo.ShipToName))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar nombre de sucursal.");
                    return;
                }

                if (string.IsNullOrEmpty(NewShipTo.Country))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes seleccionar un pais.");
                    return;
                }

                //var UltimoRegistro = dataService
                //.Get<ShipTo>(false)
                //.OrderByDescending(t => t.ShipToId)
                //.FirstOrDefault();

                //if (UltimoRegistro.ShipToId == 0)
                //{
                //    UltimoRegistro.ShipToId = 1;

                //}else{

                //    UltimoRegistro.ShipToId = UltimoRegistro.ShipToId + 1;
                //}

                var shipto = new ShipTo
                {
                    ShipToId = 0,
                    CustomerId = NewShipTo.CustomerId,
                    CustNum = EditCustomer.Customer.CustNum,
                    ShipToNum = NewShipTo.ShipToNum,
                    Company = User.Company,
                    ShipToName = NewShipTo.ShipToName,
                    TerritoryEpicorID = NewShipTo.TerritoryEpicorID,
                    Country = NewShipTo.Country,
                    State = NewShipTo.State,
                    City = NewShipTo.City,
                    Address = NewShipTo.Address,
                    PhoneNum = NewShipTo.PhoneNum,
                    Email = NewShipTo.Email,
                    SincronizadoEpicor = false,
                };



                #region Sincronizo Shipto
                var url = Application.Current.Resources["URLAPI"].ToString();

                var shiptorequest = new SyncShiptoRequest
                {
                    ShipToId = shipto.ShipToId,
                    CustomerId = shipto.CustomerId,
                    ShipToNum = shipto.ShipToNum,
                    CustNum = shipto.CustNum,
                    Company = shipto.Company,
                    ShipToName = shipto.ShipToName,
                    TerritoryEpicorID = shipto.TerritoryEpicorID,
                    Country = shipto.Country,
                    State = shipto.State,
                    City = shipto.City,
                    Address = shipto.Address,
                    PhoneNum = shipto.PhoneNum,
                    Email = shipto.Email,
                    VendorId = User.VendorId,
                    SincronizadoEpicor = false,
                };


                var response = await apiService
                                     .PostMaster(url, "/api/ShipToes", shiptorequest);

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                var syncShiptoRequest = (SyncShiptoRequest)response.Result;
                var ShipToToInsert = new ShipTo
                {
                    Address = syncShiptoRequest.Address,
                    City = syncShiptoRequest.City,
                    Company = syncShiptoRequest.Company,
                    Country = syncShiptoRequest.Country,
                    CustNum = syncShiptoRequest.CustNum,
                    CustomerId = syncShiptoRequest.CustomerId,
                    Email = syncShiptoRequest.Email,
                    PhoneNum = syncShiptoRequest.PhoneNum,
                    ShipToId = syncShiptoRequest.ShipToId,
                    ShipToName = syncShiptoRequest.ShipToName,
                    ShipToNum = syncShiptoRequest.ShipToNum,
                    SincronizadoEpicor = syncShiptoRequest.SincronizadoEpicor,
                    State = syncShiptoRequest.State,
                    TerritoryEpicorID = syncShiptoRequest.TerritoryEpicorID,
                    VendorId = User.VendorId,
                };

                dataService.Insert(ShipToToInsert);

                await dialogService.ShowMessage("Exito", "Se inserto el registro con exito.");
                await navigationService.Back();
                #endregion
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public ICommand SaveContactCommand
        {
            get { return new RelayCommand(SaveContact); }
        }

        async void SaveContact()
        {
            try
            {

                if (NewContact.PersonContact.PerConID == 0)
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes seleccionar un 'PerconID'.");
                    return;
                }

                if (string.IsNullOrEmpty(NewContact.Name))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar nombre de contacto.");
                    return;
                }


                var UltimoRegistro = dataService
                .Get<Contact>(false)
                .OrderByDescending(t => t.ContactId)
                .FirstOrDefault();

                if (UltimoRegistro.ContactId == 0)
                {
                    UltimoRegistro.ContactId = 1;

                }
                else
                {
                    UltimoRegistro.ContactId = UltimoRegistro.ContactId + 1;
                }

                var contact = new Contact
                {
                    ContactId = UltimoRegistro.ContactId,
                    PerConID = NewContact.PersonContact.PerConID,
                    ShipToId = NewContact.ShipToId,
                    Name = NewContact.PersonContact.Name,
                    Address = NewContact.PersonContact.Address1,
                    PhoneNum = NewContact.PersonContact.PhoneNum,
                    Email = NewContact.PersonContact.EMailAddress,
                    Country = NewContact.Description,
                    State = NewContact.PersonContact.State,
                    City = NewContact.PersonContact.City,
                    CreateinPhone = true,

                };

                dataService.Insert(contact);
                await dialogService.ShowMessage("Exito", "Se inserto el registro con exito.");
                await navigationService.Back();
            }

            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }

        }

        public ICommand SavePersonContactCommand
        {
            get { return new RelayCommand(SavePersonContact); }
        }

        async void SavePersonContact()
        {
            try
            {
                if (NewPersonContact.PerConID == 0)
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar PerconID.");
                    return;
                }

                if (string.IsNullOrEmpty(NewPersonContact.Name))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar nombre.");
                    return;
                }

                var UltimoRegistro = dataService
            .Get<PersonContact>(false)
            .OrderByDescending(t => t.PersonContactId)
            .FirstOrDefault();

                if (UltimoRegistro.PersonContactId == 0)
                {
                    UltimoRegistro.PersonContactId = 1;

                }
                else
                {
                    UltimoRegistro.PersonContactId = UltimoRegistro.PersonContactId + 1;
                }

                var personcontact = new PersonContact
                {
                    PersonContactId = UltimoRegistro.PersonContactId,
                    PerConID = UltimoRegistro.PersonContactId,
                    Name = NewPersonContact.Name,
                    Address1 = NewPersonContact.Address1,
                    PhoneNum = NewPersonContact.PhoneNum,
                    CellPhoneNum = NewPersonContact.CellPhoneNum,
                    EMailAddress = NewPersonContact.EMailAddress,
                    Country = NewPersonContact.Country,
                    State = NewPersonContact.State,
                    City = NewPersonContact.City,
                    CreateInPhone = true,
                };

                dataService.Insert(personcontact);
                await dialogService.ShowMessage("Exito", "Se inserto el registro con exito.");
                await navigationService.Back();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public ICommand EditShipToCommand
        {
            get { return new RelayCommand(EditarShipTo); }
        }

        async void EditarShipTo()
        {
            try
            {

                var connection = await netService.CheckConnectivity();
                if (!connection.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", connection.Message);
                    return;
                }

                var answer = await dialogService.ShowConfirm(
                    "Confirmación",
                    "¿Guardar cambios?");

                if (!answer)
                {
                    return;
                }

                if (string.IsNullOrEmpty(EditShipTo.ShipTo.ShipToName))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar nombre de sucursal.");
                    return;
                }

                if (string.IsNullOrEmpty(EditShipTo.ShipTo.Country))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes seleccionar un pais.");
                    return;
                }

                var main = MainViewModel.GetInstance();

                var ShipToToUpdate= new SyncShiptoRequest
                {
                    Address = main.EditShipTo.ShipTo.Address,
                    City = main.EditShipTo.ShipTo.City,
                    Company = main.EditShipTo.ShipTo.Company,
                    Country = main.EditShipTo.ShipTo.Country,
                    CustNum = main.EditShipTo.ShipTo.CustNum,
                    CustomerId = main.EditShipTo.ShipTo.CustomerId,
                    Email = main.EditShipTo.ShipTo.Email,
                    PhoneNum = main.EditShipTo.ShipTo.PhoneNum,
                    ShipToId = main.EditShipTo.ShipTo.ShipToId,
                    ShipToName = main.EditShipTo.ShipTo.ShipToName,
                    ShipToNum = main.EditShipTo.ShipTo.ShipToNum,
                    SincronizadoEpicor = false,
                    State = main.EditShipTo.ShipTo.State,
                    TerritoryEpicorID = main.EditShipTo.ShipTo.TerritoryEpicorID,
                    VendorId = User.VendorId,
                };


                var url = Application.Current.Resources["URLAPI"].ToString();
                var response = await apiService
                                   .PostMaster(url, "/api/ShipToes", ShipToToUpdate);

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                var syncShiptoRequest = (SyncShiptoRequest)response.Result;
                var ShipToToInsert = new ShipTo
                {
                    Address = syncShiptoRequest.Address,
                    City = syncShiptoRequest.City,
                    Company = syncShiptoRequest.Company,
                    Country = syncShiptoRequest.Country,
                    CustNum = syncShiptoRequest.CustNum,
                    CustomerId = syncShiptoRequest.CustomerId,
                    Email = syncShiptoRequest.Email,
                    PhoneNum = syncShiptoRequest.PhoneNum,
                    ShipToId = syncShiptoRequest.ShipToId,
                    ShipToName = syncShiptoRequest.ShipToName,
                    ShipToNum = syncShiptoRequest.ShipToNum,
                    SincronizadoEpicor = syncShiptoRequest.SincronizadoEpicor,
                    State = syncShiptoRequest.State,
                    TerritoryEpicorID = syncShiptoRequest.TerritoryEpicorID,
                    VendorId = User.VendorId,
                };

                dataService.Delete(main.EditShipTo.ShipTo);

                dataService.Insert(ShipToToInsert);

                await dialogService.ShowMessage("Exito", "Se actualizo el registro con exito.");
                await navigationService.Back();

                #endregion
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }


        public ICommand EditContactCommand
        {
            get { return new RelayCommand(EditarContacto); }
        }

        async void EditarContacto()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
              "Confirmación",
              "¿Guardar contacto?");

                if (!answer)
                {
                    return;
                }

                if (string.IsNullOrEmpty(EditContact.Contact.Name))
                {
                    await dialogService.ShowMessage(
                    "Error",
                    "Debes ingresar nombre de contacto.");
                    return;
                }

                dataService.Update(EditContact.Contact);
                await dialogService.ShowMessage("Exito", "Se actualizo el registro con exito.");
                await navigationService.Back();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public ICommand DeleteShipToCommand
        {
            get { return new RelayCommand(DeleteShipTo); }
        }

        async void DeleteShipTo()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Eliminar sucursal?");

                if (!answer)
                {
                    return;
                }
                dataService.Delete(EditShipTo.ShipTo);
                var EditCustomer = EditCustomerViewModel.GetInstance();
                EditCustomer.RefreshShipTo();
                await dialogService.ShowMessage("Exito", "Se elimino el registro con exito");
                await navigationService.Back();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
            await navigationService.Back();
        }

        public ICommand DeleteContactCommand
        {
            get { return new RelayCommand(DeleteContact); }
        }

        async void DeleteContact()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Eliminar contacto?");

                if (!answer)
                {
                    return;
                }
                dataService.Delete(EditContact.Contact);
                var EditShipTo = EditShipToViewModel.GetInstance();
                EditShipTo.Contacts.Remove(EditContact.Contact);
              //  EditShipTo.RefreshContacts();
                await dialogService.ShowMessage("Exito", "Se elimino el registro con exito");
                await navigationService.Back();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

    }
}

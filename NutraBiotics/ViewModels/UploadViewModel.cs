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

    public class UploadViewModel : INotifyPropertyChanged
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
        bool _subirshiptoes;
        bool _subircontactos;
        bool _seleccionartodos;

        List<ShipTo> shipTos;
        List<Contact> contacts;


        #endregion

        #region Properties

        public bool SeleccionarTodos
        {
            set
            {
                if (_seleccionartodos != value)
                {
                    _seleccionartodos = value;
                    SubirShipToes = value;
                    SubirContactos = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeleccionarTodos)));
                }
            }
            get
            {
                return _seleccionartodos;
            }
        }

        public bool SubirShipToes
        {
            set
            {
                if (_subirshiptoes != value)
                {
                    _subirshiptoes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubirShipToes)));
                }
            }
            get
            {
                return _subirshiptoes = _seleccionartodos;
            }
        }

        public bool SubirContactos
        {
            set
            {
                if (_subircontactos != value)
                {
                    _subircontactos = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubirContactos)));
                }
            }
            get
            {
                return _subircontactos;
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
        public UploadViewModel()
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
        public ICommand UploadCommand
        {
            get { return new RelayCommand(Upload); }
        }

        async void Upload()
        {
            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de iniciar una nueva carga de datos?");
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


            //var mainViewModel = MainViewModel.GetInstance();
            //DataService dataService = new DataService();
            //var Shiptolist = dataService.Get<ShipTo>(false)
            //.Where(s => s.CreateinPhone == true).ToList();
            //var url = Application.Current.Resources["URLAPI"].ToString();

            //foreach (var item in Shiptolist)
            //{
            //    var shiptorequest = new SyncShiptoRequest
            //    {
            //        ShipToId = item.ShipToId,
            //        CustomerId = item.CustomerId,
            //        ShipToNum = item.ShipToNum,
            //        CustNum = item.CustNum,
            //        Company = item.Company,
            //        ShipToName = item.ShipToName,
            //        TerritoryEpicorId = item.TerritoryEpicorId,
            //        Country = item.Country,
            //        State = item.State,
            //        City = item.City,
            //        Address = item.Address,
            //        PhoneNum = item.PhoneNum,
            //        Email = item.Email,
            //        VendorId = mainViewModel.User.VendorId,
            //        SincronizadoEpicor = false,
            //    };
            //    var answerresult = await apiService.PostMaster(url, "/ShipToes", shiptorequest);
            }


            #endregion

            #region Methods

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

            #endregion
        }
    }

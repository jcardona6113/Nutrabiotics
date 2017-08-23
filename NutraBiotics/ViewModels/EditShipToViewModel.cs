using GalaSoft.MvvmLight.Command;
using NutraBiotics.Models;
using NutraBiotics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NutraBiotics.ViewModels
{
    public class EditShipToViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        ShipTo _shipTo;
        string _selectedTerritoryEpicorId;
        string _selectedCountry;
        bool isEnabled;
        bool isRunning;
        string result;
        ObservableCollection<Contact> _contacts;
        ObservableCollection<Country> _countries;
        ObservableCollection<Territory> _territories;
        #endregion

        #region Properties

        public ObservableCollection<Country> Countries
        {
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Countries)));
                }
            }
            get
            {
                return _countries;
            }
        }

        public ObservableCollection<Territory> Territories
        {
            set
            {
                if (_territories != value)
                {
                    _territories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Territories)));
                }
            }
            get
            {
                return _territories;
            }
        }


        public string Result
        {
            set
            {
                if (result != value)
                {
                    result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
            get
            {
                return result;
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contacts)));
                }
            }
            get
            {
                return _contacts;
            }
        }


        public ShipTo ShipTo
        {
            set
            {
                if (_shipTo != value)
                {
                    _shipTo = value;
                    SelectedTerritoryEpicorId = _shipTo.TerritoryEpicorID;
                    SelectedCountry = _shipTo.Country;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipTo)));
                }
            }
            get
            {
                return _shipTo;
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

        public string SelectedTerritoryEpicorId
        {
            set
            {
                if (_selectedTerritoryEpicorId != value)
                {
                    _selectedTerritoryEpicorId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTerritoryEpicorId)));
                }
            }
            get
            {
                return _selectedTerritoryEpicorId;
            }
        }

        public string SelectedCountry
        {
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCountry)));
                }
            }
            get
            {
                return _selectedCountry;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
            get
            {
                return isRunning;
            }
        }

        #endregion

        #region Constructor

        public EditShipToViewModel()
        {
            instance = this;
            dialogService = new DialogService();
            dataService = new DataService();
            navigationService = new NavigationService();
            IsRunning = false;
        }

        #endregion

        #region Singleton
        static EditShipToViewModel instance;

        public static EditShipToViewModel GetInstance()
        {
            if (instance == null)
            {
                return new EditShipToViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods

        public void LoadCountries()
        {
            var countrieslist = dataService
                .Get<Country>(false)
                .ToList();
            Countries = new ObservableCollection<Country>(countrieslist);

        }

        public void LoadTerritories()
        {
            var territorieslist = dataService
                .Get<Territory>(false)
                .ToList();
            Territories = new ObservableCollection<Territory>(territorieslist);
        }

        public void LoadShipTo(ShipTo shipto)
        {
            this.ShipTo = shipto;
        }

        //public void LoadContacts(ObservableCollection<Contact> contacts)
        //{
        //    this._contacts = contacts;
        //}

        //public void RefreshContacts()
        //{
        //    var contacts = dataService.Get<Contact>(false)
        //    .Where(s => s.ShipToId == ShipTo.ShipToId)
        //    .ToList();
        //    Contacts = new ObservableCollection<Contact>(contacts);
        //}

        #endregion

        #region Commands

        public ICommand SaveShipToCommand
        {
            get { return new RelayCommand(SaveShipTo); }
        }

        async void SaveShipTo()
        {
            try
            {
                dataService.Update<ShipTo>(ShipTo);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
             Result = "Se actualizaron los datos con exito.";
        }


        //public ICommand NewContactCommand
        //{
        //    get { return new RelayCommand(NewContact); }
        //}

        //async void NewContact()
        //{
        //    var mainViewModel = MainViewModel.GetInstance();
        //    mainViewModel.NewContact = new NewContactViewModel();
        //    mainViewModel.NewContact.ObtenerIdShipTo(ShipTo.ShipToId);
        //    mainViewModel.NewContact.LoadCountries();
        //    await navigationService.Navigate("NewContactPage");
        //}

        #endregion

    }
}


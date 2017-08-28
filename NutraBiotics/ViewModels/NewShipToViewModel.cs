namespace NutraBiotics.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using NutraBiotics.Models;
    using NutraBiotics.Services;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class NewShipToViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationservice;
        #endregion

        #region Attributes

        string _description;
        string _territoriepicorid;
     
        ShipTo _shipTo;
        string _shiptonum;
        string _country;
        string _state;
        bool isRunning;
        string _city;
        string _address;
        string _phonenum;
        string _shiptoname;
        int _customerId;
        string _email;
        ObservableCollection<Country> _countries;
        ObservableCollection<Territory> _territories;

        #endregion

        #region Properties

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

        public string Description
        {
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
            get
            {
                return _description;
            }
        }

        public string TerritoryEpicorID
        {
            set
            {
                if (_territoriepicorid != value)
                {
                    _territoriepicorid = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TerritoryEpicorID)));
                }
            }
            get
            {
                return _territoriepicorid;
            }
        }


        public string ShipToName
        {
            set
            {
                if (_shiptoname != value)
                {
                    _shiptoname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipToName)));
                }
            }
            get
            {
                return _shiptoname;
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


        public string ShipToNum
        {
            set
            {
                if (_shiptonum != value)
                {
                    _shiptonum = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipToNum)));
                }
            }
            get
            {
                return _shiptonum;
            }
        }


        public string Country
        {
            set
            {
                if (_country != value)
                {
                    _country = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
                }
            }
            get
            {
                return _country;
            }
        }

        public string State
        {
            set
            {
                if (_state != value)
                {
                    _state = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
                }
            }
            get
            {
                return _state;
            }
        }

        public string City
        {
            set
            {
                if (_city != value)
                {
                    _city = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
            get
            {
                return _city;
            }
        }

        public int CustomerId
        {
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerId)));
                }
            }
            get
            {
                return _customerId;
            }
        }

        public string Address
        {
            set
            {
                if (_address != value)
                {
                    _address = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
                }
            }
            get
            {
                return _address;
            }
        }

        public string PhoneNum
        {
            set
            {
                if (_phonenum != value)
                {
                    _phonenum = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNum)));
                }
            }
            get
            {
                return _phonenum;
            }
        }

        public string Email
        {
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
            get
            {
                return _email;
            }
        }

        public ShipTo ShipTo
        {
            set
            {
                if (_shipTo != value)
                {
                    _shipTo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipTo)));
                }
            }
            get
            {
                return _shipTo;
            }
        }

        #endregion

        #region Constructor

        public NewShipToViewModel()
        {
            dialogService = new DialogService();
            dataService = new DataService();
            navigationservice = new NavigationService();
            Territories = new ObservableCollection<Territory>();
            IsRunning = false;

        }

        #endregion

        #region Methods

        public void LoadCountries()
        {
            var countrieslist = dataService
                .Get<Country>(false)
                .OrderBy(t => t.Description)
                .ToList();

            Countries = new ObservableCollection<Country>(countrieslist);
        }

        public void LoadTerritories()
        {
            var territorieslist = dataService
                .Get<Territory>(false)
                .OrderBy(t=> t.TerritoryDesc)
                .ToList();

            Territories = new ObservableCollection<Territory>(territorieslist);
        }

        public void ObtenerIdCliente(int customerId)
        {
            _customerId = customerId;
        }
        #endregion

        #region Commands

        #endregion

    }
}

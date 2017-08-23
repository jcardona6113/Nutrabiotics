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

    public class NewContactViewModel : INotifyPropertyChanged
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


        ObservableCollection<Country> _countries;
        string _address;
        string _phonenum;
        string _name;
        int _shiptoId;
        string _email;
        PersonContact _personContact;
        string _description;
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

        public string Name
        {
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
            get
            {
                return _name;
            }
        }

        public int ShipToId
        {
            set
            {
                if (_shiptoId != value)
                {
                    _shiptoId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipToId)));
                }
            }
            get
            {
                return _shiptoId;
            }
        }

        public PersonContact PersonContact
        {
            set
            {
                if (_personContact != value)
                {
                    _personContact = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PersonContact)));
                }
            }
            get
            {
                return _personContact;
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

        public string Address1
        {
            set
            {
                if (_address != value)
                {
                    _address = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address1)));
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

        #endregion

        #region Constructor

        public NewContactViewModel()
        {
            instance = this;
            dialogService = new DialogService();
            dataService = new DataService();
            navigationService = new NavigationService();
        }

        #endregion

        #region Singleton
        static NewContactViewModel instance;

        public static NewContactViewModel GetInstance()
        {
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

        public void ObtenerIdShipTo(int shiptoId)
        {
            _shiptoId = shiptoId;
        }


        public async Task PersonContactIdCompleted()
        {
            var personcontactid = dataService
                   .Get<PersonContact>(false)
                   .Where(s => s.PersonContactId == PersonContact.PersonContactId)
                   .FirstOrDefault();

            if (personcontactid == null)
            {
               var pregunta = await dialogService.ShowConfirm(
                    "Informacion",
                    "El PersonContactID no existe, ¿Desea crearlo?");


                if (!pregunta)
                {
                    return;
                }


                return;
            }

            this.Name = personcontactid.Name;
            this.PhoneNum = personcontactid.PhoneNum;
        }

        #endregion

        #region Commands

        public ICommand SearchPersonContactCommand
        {
            get { return new RelayCommand(SearchPersonContact); }
        }
        async void SearchPersonContact()
        {
            try
            {
                var personcontacts = dataService
                .Get<PersonContact>(false)
                .ToList();


                if (personcontacts == null || personcontacts.Count == 0)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "Debes descargar maestros.");
                    return;
                }

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.PersonContacts = new PersonContactsViewModel(personcontacts);
                mainViewModel.NewContact.PersonContact = null;
                await navigationService.Navigate("PersonContactsPage");

            }
            catch (Exception ex)
            {

                await dialogService.ShowMessage("Error", ex.Message);
            }
        }


        #endregion
    }
}

using NutraBiotics.Models;
using NutraBiotics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.ViewModels
{
    public class NewPersonContactViewModel : INotifyPropertyChanged
    {


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services

        DataService dataService;
        #endregion

        #region Attributes

        ObservableCollection<Country> _countries;
        int _perconid;
        string _name;
        string _firstname;
        string _lastname;
        string _phonenum;
        string _cellphonenum;
        string _addres1;
        string _country;
        string _state;
        string _city;
        int _countrynum;
        string _emailadress;


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

        public int PerConID {

            set
            {
                if (_perconid != value)
                {
                    _perconid = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PerConID)));
                }
            }
            get
            {
                return _perconid;
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


        public string FirstName
        {
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
            get
            {
                return _firstname;
            }
        }

        public string LastName
        {
            set
            {
                if (_lastname != value)
                {
                    _lastname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
            get
            {
                return _lastname;
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

        public string EMailAddress
        {
            set
            {
                if (_emailadress != value)
                {
                    _emailadress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EMailAddress)));
                }
            }
            get
            {
                return _emailadress;
            }
        }

        public string CellPhoneNum
        {
            set
            {
                if (_cellphonenum != value)
                {
                    _cellphonenum = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CellPhoneNum)));
                }
            }
            get
            {
                return _cellphonenum;
            }
        }

        public string Address1
        {
            set
            {
                if (_addres1 != value)
                {
                    _addres1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address1)));
                }
            }
            get
            {
                return _addres1;
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

        public int CountryNum
        {
            set
            {
                if (_countrynum != value)
                {
                    _countrynum = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountryNum)));
                }
            }
            get
            {
                return _countrynum;
            }
        }

        #endregion

        public NewPersonContactViewModel()
        {
            dataService = new DataService();
            Countries = new ObservableCollection<Country>();
        }

        #region Methods

        public void LoadCountries()
        {
            var countrieslist = dataService
                .Get<Country>(false)
                .ToList();

            Countries = new ObservableCollection<Country>(countrieslist);
        }
        #endregion

    }
}

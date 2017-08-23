using GalaSoft.MvvmLight.Command;
using NutraBiotics.Helpers;
using NutraBiotics.Models;
using NutraBiotics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NutraBiotics.ViewModels
{
    public class EditCustomerViewModel : INotifyPropertyChanged
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
        Customer _customer;
        ObservableCollection<Grouping<string, ShipTo>> _shiptoes;
        List<ShipTo> _shipToes;
        string _filter;
        ShipTo shipto;
        bool _isEnabled;
        #endregion

        #region Properties

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

        public ObservableCollection<Grouping<string, ShipTo>> ShipToes
        {
            set
            {
                if (_shiptoes != value)
                {
                    _shiptoes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipToes)));
                }
            }
            get
            {
                return _shiptoes;
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

        public string Filter
        {
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    if (string.IsNullOrEmpty(_filter))
                    {
                        ReloadShipTo();
                    }
                    else
                    {
                        Search();
                    }

                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
            get
            {
                return _filter;
            }

        }


        #endregion

        #region Constructor
        public EditCustomerViewModel(Customer customer, List<ShipTo> shipToes)
        {
            instance = this;
            this._customer = customer;
            this._shipToes = shipToes;
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            ReloadShipTo();
        }

        #endregion

        #region Singleton
        static EditCustomerViewModel instance;

        public static EditCustomerViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Methods

        void ReloadShipTo()
        {
            ShipToes = new ObservableCollection<Grouping<string, ShipTo>>(
                _shipToes
                .OrderBy(c => c.ShipToName)
                .GroupBy(c => c.ShipToName[0].ToString(), c => c)
                .Select(g => new Grouping<string, ShipTo>(g.Key, g)));
        }

        public void RefreshShipTo()
        {
            var shiptoes = dataService.Get<ShipTo>(false)
            .Where(s => s.CustomerId == Customer.CustomerId)
            .ToList();

            _shipToes = shiptoes;
        }

        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        void Search()
        {
            ShipToes = new ObservableCollection<Grouping<string, ShipTo>>(
                _shipToes
                .Where(c => c.ShipToName.ToLower().Contains(Filter.ToLower()))
                .OrderBy(c => c.ShipToName)
                .GroupBy(c => c.ShipToName[0].ToString(), c => c)
                .Select(g => new Grouping<string, ShipTo>(g.Key, g)));
        }

        public ICommand NewShipToCommand
        {
            get { return new RelayCommand(NewShipTo); }
        }

        async void NewShipTo()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.NewShipTo = new NewShipToViewModel();
            mainViewModel.NewShipTo.ObtenerIdCliente(Customer.CustomerId);
            mainViewModel.NewShipTo.LoadTerritories();
            mainViewModel.NewShipTo.LoadCountries();
            await navigationService.Navigate("NewShipToPage");
        }

        #endregion
    }
}

namespace NutraBiotics.ViewModels
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;


    public class PersonContactsViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Attributes
        bool _isRefreshing;
        ObservableCollection<Grouping<string, PersonContact>> _personcontacts;
        List<PersonContact> personContacts;
        string _filter;
        #endregion

        #region Properties
        public ObservableCollection<Grouping<string, PersonContact>> PersonContactsL
        {
            set
            {
                if (_personcontacts != value)
                {
                    _personcontacts = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(PersonContactsL)));
                }
            }
            get
            {
                return _personcontacts;
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

        public string Filter
        {
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    if (string.IsNullOrEmpty(_filter))
                    {
                        RealoadPerContact();
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
        public PersonContactsViewModel(List<PersonContact> personcontacts)
        {
            this.personContacts = personcontacts;

            dialogService = new DialogService();
            navigationService = new NavigationService();

            RealoadPerContact();
        } 
        #endregion

        #region Methods
        void RealoadPerContact()
        {
            PersonContactsL = new ObservableCollection<Grouping<string, PersonContact>>(
                personContacts
                .OrderBy(c => c.Name)
                .GroupBy(c => c.Name[0].ToString(), c => c)
                .Select(g => new Grouping<string, PersonContact>(g.Key, g)));
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        void Search()
        {
            PersonContactsL = new ObservableCollection<Grouping<string, PersonContact>>(
                personContacts
                .Where(c => c.Name.ToLower().Contains(Filter.ToLower()))
                .OrderBy(c => c.Name)
                .GroupBy(c => c.Name[0].ToString(), c => c)
                .Select(g => new Grouping<string, PersonContact>(g.Key, g)));
        }

        #endregion



    }
}
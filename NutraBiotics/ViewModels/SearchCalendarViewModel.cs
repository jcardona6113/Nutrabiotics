namespace NutraBiotics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NutraBiotics.Models;
    using NutraBiotics.Services;
    using NutraBiotics.Data;
    using NutraBiotics.Helpers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class SearchCalendarViewModel : INotifyPropertyChanged
    {

        #region Attributes

        ObservableCollection<Grouping<string, Calendar>> _calendars;
        List<Calendar> calendar;
        string _filter;
        bool _isRefreshing;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        DataService dataService;

        #endregion

        #region Properties

        public ObservableCollection<Grouping<string, Calendar>> Calendars
        {
            set
            {
                if (_calendars != value)
                {
                    _calendars = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Calendars)));
                }
            }
            get
            {
                return _calendars;
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
                        ReloadCalendars();
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
        public SearchCalendarViewModel(List<Calendar> calendars)
        {

            this.calendar = calendars;

            ReloadCalendars();

        } 
        #endregion


        #region Methods

        void ReloadCalendars()
        {
            Calendars = new ObservableCollection<Grouping<string, Calendar>>(
                calendar
                .OrderBy(s => s.Description)
                .GroupBy(s => s.Description[0].ToString(), s => s)
                .Select(g => new Grouping<string, Calendar>(g.Key, g)));
        }

        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        void Search()
        {
            Calendars = new ObservableCollection<Grouping<string, Calendar>>(
                calendar
                .Where(c => c.Description.ToLower().Contains(Filter.ToLower()))
                .OrderBy(c => c.Description)
                .GroupBy(c => c.Description[0].ToString(), c => c)
                .Select(g => new Grouping<string, Calendar>(g.Key, g)));
        }

        #endregion

    }
}

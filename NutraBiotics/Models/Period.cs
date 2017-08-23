namespace NutraBiotics.Models
{

    using System.Collections.Generic;
    using System.Windows.Input;
    using System;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;
    using ViewModels;
    using System.Linq;

    public class Period
    {

        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion


        #region Properties

        public string Names { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion




        #region Commands

        public ICommand SelectRecordCommand
        {
            get { return new RelayCommand(SelectRecord); }
        }
        async void SelectRecord()
        {
            var mainviewmodel = MainViewModel.GetInstance();
                    try
                    {
                        var searchinvoice = SearchInvoicesViewModel.GetInstance();
                        //searchinvoice.Calendar = this;
                        await navigationService.Back();
                    }

                    catch (System.Exception ex)
                    {
                        await dialogService.ShowMessage("Error", ex.Message);
                    }
            }
        }

        #endregion

    }


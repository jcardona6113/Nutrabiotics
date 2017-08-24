using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using NutraBiotics.Services;

namespace NutraBiotics.Models
{
    public class Calendar
    {
        #region Properties

        [PrimaryKey]
        public int CalendarId { get; set; }

        public string Company { get; set; }

        public string FiscalCalendarID { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion


        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion


        public Calendar()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            dataService = new DataService();
        }

        #region Commands

        public System.Windows.Input.ICommand SelectRecordCommand
        {
            get { return new GalaSoft.MvvmLight.Command.RelayCommand(SelectRecord); }
        }

        async void SelectRecord()
        {
            var mainviewmodel = ViewModels.MainViewModel.GetInstance();

            switch (mainviewmodel.EjecutadoDesde)
            {
                case "SearchInvoicesViewModel":
                    try
                    {
                        var searchinvoice = ViewModels.SearchInvoicesViewModel.GetInstance();
                        searchinvoice.Calendar = this;
                        await navigationService.Back();
                    }

                    catch (System.Exception ex)
                    {
                        await dialogService.ShowMessage("Error", ex.Message);
                    }

                    break;

                case "SearchCashHeaderPage":

                    try
                    {
                        var searchcashheader = ViewModels.SearchCashHeaderViewModel.GetInstance();
                        searchcashheader.Calendar = this;
                        await navigationService.Back();
                    }

                    catch (System.Exception ex)
                    {
                        await dialogService.ShowMessage("Error", ex.Message);
                    }

                    break;
            }

            #endregion
        }

    }
}


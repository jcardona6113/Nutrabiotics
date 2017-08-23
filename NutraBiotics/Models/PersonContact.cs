using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using NutraBiotics.Services;
using NutraBiotics.ViewModels;

namespace NutraBiotics.Models
{
    public class PersonContact
    {

        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion


        #region Properties
        [PrimaryKey]
        public int PersonContactId { get; set; }

        public int PerConID { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNum { get; set; }

        public string EMailAddress { get; set; }

        public string CellPhoneNum { get; set; }

        public string Address1 { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public int CountryNum { get; set; }

        public bool CreateInPhone { get; set; }

        #endregion

        public PersonContact()
        {
            navigationService = new NavigationService();
        }


        #region methods

        public override int GetHashCode()
        {
            return PersonContactId;
        }

        #endregion


        #region Commands

        public ICommand SelectRecordCommand
        {
            get { return new RelayCommand(SelectRecord); }
        }

        async void SelectRecord()
        {
            var mainviewmodel = MainViewModel.GetInstance();
            mainviewmodel.NewContact.PersonContact = this;
            await navigationService.Back();
        }


        public ICommand EditPersonContactCommand
        {
            get { return new RelayCommand(EditPersonContact); }
        }

        async void EditPersonContact()
        {

             
        }

    }

    #endregion
}

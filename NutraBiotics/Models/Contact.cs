namespace NutraBiotics.Models
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using ViewModels;
    using SQLite.Net.Attributes;
    using System;
    using System.Linq;

    public class Contact
    {
        #region Services
        NavigationService navigationService;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Properties
        [PrimaryKey]
        public int ContactId { get; set; }

        public int PerConID { get; set; }

        public int ConNum { get; set; }

        public int ShipToId { get; set; }

        public string ShipToNum { get; set; }

        public int CustNum { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public bool SincronizadoEpicor { get; set; }

        public bool CreateinPhone { get; set; }

        #endregion

        #region Constructor
        public Contact()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ContactId;
        }
        #endregion

        #region Commands

        public ICommand SelectRecordCommand
        {
            get { return new RelayCommand(SelectRecord); }
        }

        async void SelectRecord()
        {
            var newOrderViewModel = NewOrderViewModel.GetInstance();
            newOrderViewModel.Contact = this;
            await navigationService.Back();
        }

        public ICommand EditContactCommand
        {
            get { return new RelayCommand(EditContact); }
        }

        async void EditContact()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.EditContact = new EditContactViewModel(this);
                await navigationService.Navigate("EditContactPage");
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }

        }

        public ICommand DeleteContactCommand
        {
            get { return new RelayCommand(DeleteContact); }
        }

        async void DeleteContact()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Eliminar Contacto?");

                if (!answer)
                {
                    return;
                }

                dataService.Delete(this);
                var EditShipTo = EditShipToViewModel.GetInstance();
                EditShipTo.Contacts.Remove(this);
               // EditShipTo.RefreshContacts();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion
    }
}
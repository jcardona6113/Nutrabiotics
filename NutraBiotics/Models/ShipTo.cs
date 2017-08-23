namespace NutraBiotics.Models
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using ViewModels;
    using SQLite.Net.Attributes;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using NutraBiotics.Helpers;

    public class ShipTo
    {

        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Properties

        [PrimaryKey]
        public int ShipToId { get; set; }

        public int CustomerId { get; set; }

        public string ShipToNum { get; set; }

        public int CustNum { get; set; }

        public string Company { get; set; }

        public string ShipToName { get; set; }

        public string TerritoryEpicorID { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public string VendorId { get; set; }

        public bool SincronizadoEpicor { get; set; }

        #endregion

        #region Constructor
        public ShipTo()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ShipToId;
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
            newOrderViewModel.ShipTo = this;
            await navigationService.Back();
        }

        public ICommand EditShipToCommand
        {
            get { return new RelayCommand(EditShipTo); }
        }

        async void EditShipTo()
        {
            try
            {
                var contacslist = dataService
                .Get<Contact>(false)
                .Where(s => s.ShipToId == ShipToId)
                .ToList();

                var contacts = new ObservableCollection<Contact>();
                foreach (var ind in contacslist)
                {
                    contacts.Add(new Contact
                    {
                        Name = ind.Name,
                        Address= ind.Address,
                        PhoneNum = ind.PhoneNum,
                        Email = ind.Email,
                    });
                }
                var mainViewModel = MainViewModel.GetInstance();

                mainViewModel.EditShipTo = new EditShipToViewModel();
                mainViewModel.EditShipTo.LoadCountries();
                mainViewModel.EditShipTo.LoadTerritories();
                mainViewModel.EditShipTo.LoadShipTo(this);
               // mainViewModel.EditShipTo.LoadContacts(contacts);
                await navigationService.Navigate("EditShipToPage");
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);

            }
        }


        public ICommand DeleteShipToCommand
        {
            get { return new RelayCommand(DeleteShipTo); }
        }

        async void DeleteShipTo()
        {
            try
            {
                var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Eliminar sucursal?");

                if (!answer)
                {
                    return;
                }

                dataService.Delete(this);
                var EditCustomer = EditCustomerViewModel.GetInstance();
                EditCustomer.RefreshShipTo();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

    }
}



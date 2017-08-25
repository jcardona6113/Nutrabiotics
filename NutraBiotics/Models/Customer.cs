namespace NutraBiotics.Models
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;
    using ViewModels;
    using System.Linq;

    public class Customer
    {
        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Properties
        [PrimaryKey]
        public int CustomerId { get; set; }

        public int CustNum { get; set; }

        public string CustId { get; set; }

        public string Company { get; set; }

        public string ResaleId { get; set; }

        public string TerritoryId { get; set; }

        public string ShipViaCode { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNum { get; set; }

        public string Names { get; set; }

        public string LastNames { get; set; }

        public bool CreditHold { get; set; }

        public string TermsCode { get; set; }

        public string Terms { get; set; }

                public string VendorId { get; set; }


        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<OrderHeader> OrderHeaders { get; set; }
        #endregion

        #region Constructor
        public Customer()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return CustomerId;
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

            switch (mainviewmodel.EjecutadoDesde)
            {
                case "SearchInvoicesViewModel":
                    try
                    {
                        var searchinvoice = SearchInvoicesViewModel.GetInstance();
                        searchinvoice.Calendar = null;
                        searchinvoice.Customer = this;
                        searchinvoice.InvoiceHeaders.Clear();
                        searchinvoice.TotalLineas = 0;
                        searchinvoice.IsEnabled = true;
                        searchinvoice.FiltroFechasIsEnabled = true;
                        await navigationService.Back();
                    }
                    catch (System.Exception ex)
                    {
                        await dialogService.ShowMessage("Error", ex.Message);
                    }

                    break;

                case "NewOrderViewModel":
                    try
                    {
                        if (CreditHold == true)
                        {
                            var answer = await dialogService.ShowConfirm("Informacion",
                                                             "El cliente tiene credito retenido. ¿Desea continuar?");
                            if (!answer)
                            {
                                return;
                            }
                        }
                        var newOrderViewModel = NewOrderViewModel.GetInstance();
                        newOrderViewModel.Customer = this;
                        newOrderViewModel.LoadPriceLists();
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

                        var searchvashheaderviewmodel = SearchCashHeaderViewModel.GetInstance();
                        searchvashheaderviewmodel.Calendar = null;
                        searchvashheaderviewmodel.Customer = this;
                        searchvashheaderviewmodel.TotalLineas = 0;
                        searchvashheaderviewmodel.CashHeaders.Clear();
                        searchvashheaderviewmodel.IsEnabled = true;
                        await navigationService.Back();
                    }

                    catch (System.Exception ex)
                    {
                        await dialogService.ShowMessage("Error", ex.Message);
                    }

                    break;
            }
        }


        public ICommand SelectCustomerCommand
        {
            get { return new RelayCommand(SelectCustomer); }
        }
        async void SelectCustomer()
        {
            try
            {
                var shipToes = dataService
                .Get<ShipTo>(false)
                .Where(s => s.CustomerId == CustomerId)
                .ToList();

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.EditCustomer = new EditCustomerViewModel(this, shipToes);
                await navigationService.Navigate("EditCustomerPage");
            }
            catch (System.Exception ex)
            {
                await dialogService.ShowMessage("Error",
                ex.Message);
            }
            #endregion

        }
    }
}

namespace NutraBiotics.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using NutraBiotics.Models;
    using System.ComponentModel;

    public class MenuItemViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        DialogService dialogService;
        #endregion

        #region Attributes
        NavigationService navigationService;
        DataService dataService;
        Customer _customer;
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

        public string Icon
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string PageName
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        async void Navigate()
        {
            if (PageName == "LoginPage")
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.User.IsRemembered = false;
                dataService.Update(mainViewModel.User);
                navigationService.SetMainPage("LoginPage");
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();

                switch (PageName)
                {
                    case "DownloadPage":
                        mainViewModel.Download = new DownloadViewModel();
                        break;

                    case "CustomersPage":

                        var customers = dataService.Get<Customer>(false);
                        var shiptes = dataService.Get<ShipTo>(false);
                        var countries = dataService.Get<Country>(false);
                        var territories = dataService.Get<Territory>(false);

                        if (customers == null || customers.Count == 0)
                        {
                            await dialogService.ShowMessage("Error", "Debes descargar clientes");
                            return;
                        }

                        if (countries == null || countries.Count == 0)
                        {
                            await dialogService.ShowMessage("Error", "Debes descargar paises");
                            return;
                        }

                        if (territories == null || territories.Count == 0)
                        {
                            await dialogService.ShowMessage("Error", "Debes descargar territorios");
                            return;
                        }

                        mainViewModel.CustomersVM = new CustomersViewModel(customers);
                        break;

                    case "TabPursePage":

                        var invoices = dataService.Get<InvoiceHeader>(false);
                        var cashheaders = dataService.Get<CashHeader>(false);



                        if (invoices == null || invoices.Count == 0)
                        {
                            await dialogService.ShowMessage("Error", "Debes descargar facturas");
                            return;
                        }


                        if (cashheaders == null && cashheaders.Count == 0)
                        {
                            await dialogService.ShowMessage("Error", "Debes descargar pagos");
                            return;
                        }


                        mainViewModel.SearchInvoices = new SearchInvoicesViewModel();

                        mainViewModel.SearchCashHeader = new SearchCashHeaderViewModel(cashheaders);

                        break;

                    case "UploadPage":
                        mainViewModel.Upload = new UploadViewModel();
                        break;
                }

                await navigationService.Navigate(PageName);
            }
        }
        #endregion
    }
}

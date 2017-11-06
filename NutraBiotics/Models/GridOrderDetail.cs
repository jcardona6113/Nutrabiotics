namespace NutraBiotics.Models
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using ViewModels;
    using NutraBiotics.Views;
    using Xamarin.Forms;

    public class GridOrderDetail
    {
        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Properties

        public int SalesOrderDetaliId { get; set; }

        public int PartId { get; set; }

        public int PriceListPartId { get; set; }

        public string PartNum { get; set; }

        public string PartDescription { get; set; }

        public int OrderLine { get; set; }

        public decimal BasePrice { get; set; }

        public double Quantity { get; set; }

        public double Discount { get; set; }

        public string Reference { get; set; } // U. Bonificadas

        public decimal Value
        {
            get
            {
                return BasePrice * (decimal)Quantity;
            }
        }

        #endregion

        #region Constructors

        public GridOrderDetail()
        {
   
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }

        async void DeleteProduct()
        {
			var answer = await dialogService.ShowConfirm(
				"Confirmación",
				"¿Está seguro de borrar el registro?");

			if (answer)
			{
                var newOrderViewModel = NewOrderViewModel.GetInstance();
                newOrderViewModel.GridOrderDetails.Remove(this);
                newOrderViewModel.TotalLineas = newOrderViewModel
                .GridOrderDetails.Sum(god => god.Value);
			}
		}

        public ICommand EditProductCommand
        {
            get { return new RelayCommand(EditProduct); }
        }

         async void EditProduct()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditOrderDetails = new EditOrderDetailsViewModel();
            mainViewModel.NewOrder.BeginEdit(this);
            mainViewModel.EditOrderDetails.RecibirDetalle(this);
            await navigationService.Navigate("EditOrderDetailsPage");
        }


        #endregion
    }
}

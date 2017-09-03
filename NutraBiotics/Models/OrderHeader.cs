namespace NutraBiotics.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;
    using Services;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using ViewModels;

    public class OrderHeader
    {
        #region Services
        DialogService dialogService;
        DataService dataService;
        NavigationService navigationService;
        ApiService apiService;
        #endregion

        #region Properties
        [PrimaryKey, AutoIncrement]

        public int SalesOrderHeaderId { get; set; }

        public int SalesOrderHeaderInterId { get; set; }

        public string Platform { get; set; }

        public int OrderNum { get; set; }  //Epicor

        public int UserId { get; set; }

                public string VendorId { get; set; }

        [ForeignKey(typeof(Customer))]
        public int CustomerId { get; set; }

        public string CustId { get; set; }

        public bool CreditHold { get; set; }

        public DateTime Date { get; set; }

        public DateTime NeedByDate { get; set; }

        public string TermsCode { get; set; }

        public int ShipToId { get; set; }

        public int ContactId { get; set; }

        public int ConNum { get; set; }   //Epicor

        public string ShipToNum { get; set; }

        public string SalesCategory { get; set; }

        public string Observations { get; set; }

        public decimal TaxAmt { get; set; }

        public bool IsSync { get; set; }

        public bool SincronizadoEpicor { get; set; }

        public string RowMod { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<OrderDetail> OrderDetails { get; set; }

        [ManyToOne]
        public Customer Customer { get; set; }

        public int InvoiceNum { get; set; }

        public bool Facturado { get; set; }

        public decimal Total { get; set; }


        public decimal TotalLineas
        {

            get
            {
                if (OrderDetails == null)
                {
                    return 0;
                }

                return OrderDetails.Sum(od => od.Total);
            }

            set
            {

            }

        }
        #endregion

        #region Constructors
        public OrderHeader()
        {
            dialogService = new DialogService();
            dataService = new DataService();
            navigationService = new NavigationService();
            apiService = new ApiService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return SalesOrderHeaderId;
        }

        #endregion

        #region Commands

        public ICommand EditOrderCommand
        {
            get { return new RelayCommand(EditOrder); }
        }


        async void EditOrder()
        {
            if (RowMod == "D")
            {
                await dialogService.ShowMessage(
                    "Informacion",
                    "La orden ya fue eliminada, no se puede editar.");
                return;
            }


            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.NewOrder = new NewOrderViewModel();
            mainViewModel.NewOrder.RowMod = "U";








            mainViewModel.NewOrder.EditOrderReceipt(this);
            await navigationService.Navigate("NewOrderTab");
        }


        public ICommand DeleteOrderCommand
        {
            get { return new RelayCommand(DeleteOrder); }
        }

        async void DeleteOrder()
        {
            var answer = await dialogService.ShowConfirm(
                "Confirmación",
                "¿Está seguro de eliminar la orden?");

            if (!answer)
            {
                return;
            }

            //if (IsSync == false)
            //{
            //    dataService.Delete(this);
            //    var ordersViewModel = OrdersViewModel.GetInstance();
            //    ordersViewModel.OrderList.Remove(this);

            //}else{

            var mainViewModel = MainViewModel.GetInstance();
            var ordersViewModel = OrdersViewModel.GetInstance();
            mainViewModel.NewOrder = new NewOrderViewModel();
            mainViewModel.NewOrder.RowMod = "D";
            mainViewModel.NewOrder.DeleteOrderReceipt(this);
            mainViewModel.ExecDeleteOrder();
            ordersViewModel.RefreshOrders();

            //}

            #endregion

        }
    }
}

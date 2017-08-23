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
    using System;

    public class InvoiceHeader
    {
        #region Services
        NavigationService navigationService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Properties

        [PrimaryKey]
        public int InvoiceHeaderId { get; set; }

        public string Company { get; set; }

        public int CustNum { get; set; }

        public string CustID { get; set; }

        public string Names { get; set; }

        public string InvoiceNum { get; set; }

        public string InvoiceType { get; set; }

        public decimal InvoiceAmt { get; set; }

        public decimal InvoiceBal { get; set; }

        public bool CheckSaldo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int DiasVencimiento { get; set; }

        public bool OpenInvoice { get; set; }

        #endregion

        #region Constructor

        public InvoiceHeader()
        {

            navigationService = new NavigationService();
            dataService = new DataService();
            dialogService = new DialogService();
        }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return InvoiceHeaderId;
        }
        #endregion

        #region Commands

        public ICommand ViewInvoiceDetailCommand
        {
            get { return new RelayCommand(ViewInvoiceDetail); }
        }


        async void ViewInvoiceDetail()
        {

            try
            {
                var detail_list = dataService
                .Get<InvoiceDetail>(false)
                .Where(s => s.InvoiceNum == InvoiceNum)
                .ToList();

                var details = new System.Collections.ObjectModel.ObservableCollection<InvoiceDetail>();
                foreach (var ind in detail_list)
                {
                    details.Add(new InvoiceDetail
                    {
                        PartNum = ind.PartNum,
                        InvoiceNum = ind.InvoiceNum,
                        OurShipQty = ind.OurShipQty,
                        UnitPrice = ind.UnitPrice,
                        PartDescription = ind.PartDescription,
                    });
                }

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.InvoiceDetail = new InvoiceDetailViewModel(details);
                await navigationService.Navigate("InvoiceDetailPage");
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion

    }
}

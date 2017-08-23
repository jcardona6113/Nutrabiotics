namespace NutraBiotics.Models
{
    public class SynOrderDetailRequest
    {

        public int SalesOrderDetaliId { get; set; }

        public int SalesOrderHeaderId { get; set; }

        public int OrderNum { get; set; }  //Epicor

        public int PriceListPartId { get; set; }

        public int PartId { get; set; }

        public int OrderLine { get; set; }

        public string PartNum { get; set; }

        public double OrderQty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TaxAmt { get; set; }

        public string Reference { get; set; }

        public decimal Total { get; set; }

    }
}

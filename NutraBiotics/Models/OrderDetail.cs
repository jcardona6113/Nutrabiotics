namespace NutraBiotics.Models
{
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;


    public class OrderDetail
	{
		[PrimaryKey, AutoIncrement]
		public int SalesOrderDetaliId { get; set; }

        [ForeignKey(typeof(OrderHeader))]

        public int SalesOrderHeaderId { get; set; }

        public int OrderNum { get; set; }  //Epicor

        public int PriceListPartId { get; set; }

        public int PartId { get; set; }

		public int OrderLine { get; set; }

		public string PartNum { get; set; }

		public double OrderQty { get; set; }

		public decimal UnitPrice { get; set; }

		public decimal TaxAmt { get; set; }

        public string Reference { get; set; } //U. Bonificadas


        [ManyToOne]
		public OrderHeader OrderHeader { get; set; }

		public decimal Total
        {
			get
			{
				return UnitPrice * (decimal)OrderQty;
			}
		}


        public override int GetHashCode()
        {
            return SalesOrderDetaliId;
        }
	}
}
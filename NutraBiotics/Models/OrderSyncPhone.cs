using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
    public class OrderSyncPhone
    {

        public int SalesOrderHeaderPhoneId { get; set; }

        public int SalesOrderHeaderInterId { get; set; }

        public int OrderNum { get; set; }

        public decimal TaxAmt { get; set; }

        public decimal Total { get; set; }

        public string Platform { get; set; }

        public int InvoiceNum { get; set; }

        public bool Facturado { get; set; }

    }
}

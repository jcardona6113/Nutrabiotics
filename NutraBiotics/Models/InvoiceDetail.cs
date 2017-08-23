using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
    public class InvoiceDetail
    {

            [PrimaryKey]
            public int InvoiceDetailId { get; set; }

            public int InvoiceHeaderId { get; set; }

            public string InvoiceType { get; set; }

            public string InvoiceNum { get; set; }

            public int InvoiceLine { get; set; }

            public string PartNum { get; set; }

        public string PartDescription { get; set; }
        
        public decimal OurShipQty { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal ExtPrice { get; set; }

            public decimal TaxAmt { get; set; }

            public decimal DocTaxAmt { get; set; }


        public decimal Value
        {
            get
            {
                return UnitPrice * (decimal)OurShipQty;
            }
        }

    }

}


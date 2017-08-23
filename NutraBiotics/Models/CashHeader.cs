using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
    public class CashHeader
    {

        [PrimaryKey]

        public int CashHeaderId { get; set; }

        public int HeadNum { get; set; }

        public bool Posted { get; set; }

        public string TranType { get; set; }

        public int InvoiceNum { get; set; }

        public decimal TranAmt { get; set; }

        public decimal DocTranAmt { get; set; }

        public string CustId { get; set; }

        public int Custnum { get; set; }

        public string Names { get; set; }

        public DateTime TranDate { get; set; }

        public decimal AppliedAmt { get; set; }

        public decimal DocAppliedAmt { get; set; }

    }
}

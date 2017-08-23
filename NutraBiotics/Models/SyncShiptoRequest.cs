using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
   public class SyncShiptoRequest
    {

        [PrimaryKey]
        public int ShipToId { get; set; }

        public int CustomerId { get; set; }

        public string ShipToNum { get; set; }  //Epicor

        public int CustNum { get; set; } //Epicor

        public string Company { get; set; }

        public string ShipToName { get; set; }

        public string TerritoryEpicorID { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public string VendorId { get; set; }

        public bool SincronizadoEpicor { get; set; }
    }
}

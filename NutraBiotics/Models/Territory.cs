using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
   public  class Territory
    {
        [PrimaryKey]
        public int TerritoryID { get; set; }

        public string Company { get; set; }

        public string TerritoryEpicorID { get; set; }

        public string TerritoryDesc { get; set; }

        public string RegionCode { get; set; }

    }
}

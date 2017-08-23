using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.Models
{
    public class Country
    {

        [PrimaryKey]
        public int CountryId { get; set; }

        public string Company { get; set; }

        public int CountryNum { get; set; }

        public string FiscalCode { get; set; }

        public string Description { get; set; }

    }
}

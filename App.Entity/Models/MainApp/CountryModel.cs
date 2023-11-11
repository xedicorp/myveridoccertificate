using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Models.MainApp
{
    public class CountryModel
    {
        public int COUNTRY_ID { get; set; }
        public string? ISO { get; set; }
        public string? NAME { get; set; }
        public string? NICENAME { get; set; }
        public string? ISO3 { get; set; }
        public int NUMCODE { get; set; }
        public int PHONECODE { get; set; }
    }
}

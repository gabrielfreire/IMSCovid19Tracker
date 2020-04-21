using System;
using System.Collections.Generic;
using System.Text;

namespace IMSCovidTracker.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Cioc { get; set; }
        public Int64 Population { get; set; }
        public string Capital { get; set; }
    }
}

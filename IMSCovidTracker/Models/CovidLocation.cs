using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMSCovidTracker.Models
{
    public class CovidLocation
    {
        [JsonProperty("Province_State")]
        public string Province { get; set; }
        [JsonProperty("Country_Region")]
        public string Country { get; set; }
        public int Deaths { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
    }
}

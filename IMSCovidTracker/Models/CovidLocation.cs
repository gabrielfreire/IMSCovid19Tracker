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
        public int Active { get; set; }
        public int DeathsPerMillion { get; set; } = 0;
        public Int64 TotalPopulation { get; set; }
        //public int Active => (Confirmed - Recovered) - Deaths;
        public double ConfirmedPerRecovered => (double) Recovered / Confirmed * 100;
        public double ConfirmedPerDeaths => Deaths < Confirmed ? (double)Deaths / Confirmed * 100 : (double)Confirmed / Deaths * 100;
        public double RecoveredPerDeaths =>  Deaths < Recovered ? (double)Deaths / Recovered * 100 : (double)Recovered / Deaths * 100;
    }
}

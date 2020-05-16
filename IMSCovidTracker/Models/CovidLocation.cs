﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMSCovidTracker.Models
{
    public class CovidLocation
    {
        public string Province { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string FlagImageUrl { get; set; }
        public long Deaths { get; set; } = 0;
        public long Confirmed { get; set; } = 0;
        public long Recovered { get; set; } = 0;
        public long Active { get; set; } = 0;
        public double DeathsPerMillion { get; set; } = 0;
        public long TotalPopulation { get; set; } = 0;
        //public int Active => (Confirmed - Recovered) - Deaths;
        public double ConfirmedPerRecovered => (double) Recovered / Confirmed * 100;
        public double ConfirmedPerDeaths => Deaths < Confirmed ? (double)Deaths / Confirmed * 100 : (double)Confirmed / Deaths * 100;
        public double RecoveredPerDeaths =>  Deaths < Recovered ? (double)Deaths / Recovered * 100 : (double)Recovered / Deaths * 100;
    }
}

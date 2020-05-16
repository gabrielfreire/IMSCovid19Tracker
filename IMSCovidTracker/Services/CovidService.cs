using IMSCovidTracker.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IMSCovidTracker.Services
{
    public class CovidService
    {
        public IEnumerable<CovidLocation> CovidLocations { get; set; }
        
        public async Task GetLocationsAsync()
        {
            try
            {
                CovidLocations = await App.WorldmeterScraperService.ScrapeCountriesCovidCases();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                throw ex;
            }
        }

        public async Task<CovidLocation> GetTotalCases()
        {
            var cLoc = default(CovidLocation);
            try
            {
                cLoc = await App.WorldmeterScraperService.ScrapeTotalCovidCases();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                throw ex;
            }

            return cLoc;
        }

        public CovidLocation Find(string countryName)
        {
            if (CovidLocations == null) return default(CovidLocation);
            if (string.IsNullOrEmpty(countryName)) return default(CovidLocation);

            var _results = CovidLocations
                            .Where(l => l.Country.ToLower() == countryName.ToLower());

            return _results.FirstOrDefault();
        }

        public IEnumerable<CovidLocation> FindPartial(string searchQuery)
        {
            if (CovidLocations == null) return new List<CovidLocation>();
            if (string.IsNullOrEmpty(searchQuery)) return new List<CovidLocation>();
            
            return CovidLocations.Where(l => l.Country.ToLower()
                                            .Contains(searchQuery.ToLower()))
                                            .ToList();
        }
    }
}

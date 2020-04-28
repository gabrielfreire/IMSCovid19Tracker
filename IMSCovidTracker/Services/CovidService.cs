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
        
        
        public CovidService()
        {
        }

        public async Task GetLocationsAsync()
        {
            try
            {
                CovidLocations = await App.WorldmeterScraperService.ScrapeCountriesCovidCases();
            }
            catch (Exception ex)
            {
                //TODO: Notify error
                App.MessageDialogService.Display("Error", $"An exception was thrown {ex.ToString()}");
            }
        }

        public Task<CovidLocation> GetTotalCases()
        {
            return App.WorldmeterScraperService.ScrapeTotalCovidCases();
        }

        public CovidLocation Find(string countryName)
        {
            if (CovidLocations == null) return default(CovidLocation);
            if (string.IsNullOrEmpty(countryName)) return default(CovidLocation);

            var _results = CovidLocations.Where(l => l.Country.ToLower() == countryName.ToLower());

            return _results.FirstOrDefault();
        }

        public IEnumerable<CovidLocation> FindPartial(string searchQuery)
        {
            var _results = new List<CovidLocation>();

            if (string.IsNullOrEmpty(searchQuery)) return _results;
            
            _results = CovidLocations.Where(l => l.Country.ToLower().Contains(searchQuery.ToLower())).ToList();
            return CovidLocations.Where(l => l.Country.ToLower()
                                            .Contains(searchQuery.ToLower()))
                                            .ToList();
        }
    }
}

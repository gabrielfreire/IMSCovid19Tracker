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
        private HttpClient _httpClient;
        //private string _apiEndpoint = "https://services1.arcgis.com/0MSEUqKaxRlEPj5g/arcgis/rest/services/ncov_cases/FeatureServer/1/query";
        //private string _queryParams = "?f=json&where=(Confirmed%3E%200)%20OR%20(Deaths%3E0)%20OR%20(Recovered%3E0)&returnGeometry=false&outFields=*&orderByFields=Country_Region%20asc,Province_State%20asc&resultOffset=0";
        
        public IEnumerable<CovidLocation> CovidLocations { get; set; }
        
        
        public CovidService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

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
            //if (_results.Count() == 0) return default(CovidLocation);
            
            //var _final = new CovidLocation() { Country = countryName };
            //_final.Country = _results.First().Country;
            //_final.CountryCode = _results.First().CountryCode;
            //_final.FlagImageUrl = _results.First().FlagImageUrl;

            //foreach (var res in _results)
            //{
            //    _final.Confirmed += res.Confirmed;
            //    _final.Deaths += res.Deaths;
            //    _final.Recovered += res.Recovered;
            //    _final.Active += res.Active;
            //    _final.DeathsPerMillion += res.DeathsPerMillion;
            //    _final.TotalPopulation += res.TotalPopulation;
            //}

            //return _final;
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

using IMSCovidTracker.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IMSCovidTracker.Services
{
    public class CovidService
    {
        private HttpClient _httpClient;
        private string _apiEndpoint = "https://services1.arcgis.com/0MSEUqKaxRlEPj5g/arcgis/rest/services/ncov_cases/FeatureServer/1/query";
        private string _queryParams = "?f=json&where=(Confirmed%3E%200)%20OR%20(Deaths%3E0)%20OR%20(Recovered%3E0)&returnGeometry=false&outFields=*&orderByFields=Country_Region%20asc,Province_State%20asc&resultOffset=0";
        
        public IEnumerable<CovidLocation> CovidLocations { get; set; }
        
        
        public CovidService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        }

        public async Task<IEnumerable<CovidLocation>> GetLocationsAsync()
        {
            var locations = new List<CovidLocation>();
            try
            {
                var result = await _httpClient.GetAsync($"{_apiEndpoint}{_queryParams}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    // TODO: parse to JSON
                    dynamic _jsonData = JObject.Parse(content);
                    var features = _jsonData["features"];

                    if (features != null)
                    {
                        foreach (var feature in features)
                        {
                            locations.Add(new CovidLocation
                            {
                                Country = feature["attributes"]["Country_Region"],
                                Province = feature["attributes"]["Province_State"],
                                Deaths = feature["attributes"]["Deaths"],
                                Confirmed = feature["attributes"]["Confirmed"],
                                Recovered = feature["attributes"]["Recovered"]
                            });
                        }
                    }
                }
                else
                    App.MessageDialogService.Display("Error", "The request to Arcgis API failed");

                CovidLocations = locations;

                return locations;

            }
            catch (Exception ex)
            {
                //TODO: Notify error
                App.MessageDialogService.Display("Error", $"An exception was thrown {ex.ToString()}");
                return locations;
            }
        }

        public CovidLocation GetTotalCases(IEnumerable<CovidLocation> locations)
        {
            var _totalCases = new CovidLocation
            {
                Confirmed = 0,
                Deaths = 0,
                Recovered = 0
            };

            foreach (var location in locations)
            {
                _totalCases.Confirmed += location.Confirmed;
                _totalCases.Deaths += location.Deaths;
                _totalCases.Recovered += location.Recovered;
            }
            return _totalCases;
        }

        public CovidLocation Find(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return default(CovidLocation);
            var _results = CovidLocations.Where(l => l.Country.ToLower() == countryName.ToLower());
            var _final = new CovidLocation() { Country = countryName };

            if (_results.Count() == 0) return default(CovidLocation);

            _final.Country = _results.First().Country;
            foreach (var res in _results)
            {
                _final.Confirmed += res.Confirmed;
                _final.Deaths += res.Deaths;
                _final.Recovered += res.Recovered;
            }
            return _final;
        }
    }
}

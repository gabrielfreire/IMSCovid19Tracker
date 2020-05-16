using IMSCovidTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMSCovidTracker.Services
{
    public class WorldmeterScraperService
    {
        private string _host = "http://covidrest.azurewebsites.net";
        //private string _host = "http://192.168.0.220";
        //private IBrowsingContext _context;
        private HttpClient _httpClient;

        public WorldmeterScraperService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(20);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        }
        
        public async Task<Int64> ScrapeWorldPopulation()
        {
           
            Int64 population = default;
            try
            {
                
                var _result = await _httpClient.GetAsync($"{_host}/v1/covid/worldpopulation");
                if (_result.IsSuccessStatusCode)
                {
                    var content = await _result.Content.ReadAsStringAsync();
                    population = JsonConvert.DeserializeObject<long>(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return population;
        }

       
        public async Task<CovidLocation> ScrapeTotalCovidCases()
        {
            var _covidLocation = new CovidLocation
            {
                Confirmed = 0,
                Deaths = 0,
                Recovered = 0
            };

            try
            {
                
                var _result = await _httpClient.GetAsync($"{_host}/v1/covid/world");
                if (_result.IsSuccessStatusCode)
                {
                    var _content = await _result.Content.ReadAsStringAsync();
                    _covidLocation = JsonConvert.DeserializeObject<CovidLocation>(_content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _covidLocation;
        }

        public async Task<IEnumerable<CovidLocation>> ScrapeCountriesCovidCases()
        {
            var _covidLocations = new List<CovidLocation>();
            try
            {
                var _result = await _httpClient.GetAsync($"{_host}/v1/covid/countries");
                if (_result.IsSuccessStatusCode)
                {
                    var _content = await _result.Content.ReadAsStringAsync();
                    _covidLocations = JsonConvert.DeserializeObject<List<CovidLocation>>(_content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _covidLocations;
        }
    }
}

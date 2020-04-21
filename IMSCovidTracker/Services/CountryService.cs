using IMSCovidTracker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMSCovidTracker.Services
{
    public class CountryService
    {
        
        private HttpClient _httpClient;
        private string _apiEndpoint = "https://restcountries.eu/rest/v2";
        private string _queryParams = "?fulltext=true";

        public IEnumerable<Country> Countries { get; set; } = new List<Country>();

        public CountryService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        }

        public async Task<IEnumerable<Country>> GetCountriesInfo()
        {
            try
            {
                var _result = await _httpClient.GetAsync($"{_apiEndpoint}/all");
                var _content = await _result.Content.ReadAsStringAsync();
                if (_result.IsSuccessStatusCode)
                {
                    var _countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(_content);
                    Countries = _countries;
                    return _countries;
                }
                throw new Exception(_content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Int64> GetTotalPopulation()
        {
            var countries = await GetCountriesInfo();
            var _totalPopulation = (long) countries.Select<Country, Int64>(c => c.Population).Sum();
            return _totalPopulation;
        }
        

        public async Task<int> GetTotalPopulation(string countryName)
        {
            try
            {
                var _result = await _httpClient.GetAsync($"{_apiEndpoint}/name/{ParseCountryName(countryName)}{_queryParams}");
                var _content = await _result.Content.ReadAsStringAsync();
                if (_result.IsSuccessStatusCode)
                {
                    var _data = JArray.Parse(_content);
                    var _population = _data[0].Value<int>("population");
                    return _population;
                }
                throw new Exception(_content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ParseCountryName(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return "";
            if (countryName.ToLower().Contains("korea") && countryName.ToLower().Contains("south"))
            {
                return "Korea (Republic of)";
            }
            return countryName;
        }

    }
}

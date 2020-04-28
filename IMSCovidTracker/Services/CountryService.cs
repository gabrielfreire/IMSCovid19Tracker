using IMSCovidTracker.Models;
using AngleSharp;
using AngleSharp.Html.Dom;
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
using AngleSharp.Dom;
using System.Diagnostics;

namespace IMSCovidTracker.Services
{
    public class CountryService
    {

        private HttpClient _httpClient;
        private string _apiEndpoint = "https://restcountries.eu/rest/v2";
        private string _queryParams = "?fullText=true";

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

        public Task<Int64> GetTotalPopulation()
        {
            return App.WorldmeterScraperService.ScrapeWorldPopulation();
        }
    }
}
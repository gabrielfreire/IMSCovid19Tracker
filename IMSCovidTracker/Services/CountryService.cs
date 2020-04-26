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

        private IBrowsingContext _context;

        public Dictionary<string, Int64> _CountryPopulationDict = new Dictionary<string, Int64>();

        public IEnumerable<Country> Countries { get; set; } = new List<Country>();

        public CountryService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            // anglesharp for scraping the web
            //var config = Configuration.Default.WithDefaultLoader();
            _context = BrowsingContext.New(Configuration.Default);

            _ = _getAllCountriesPopulationFromWebscraping();

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
            var _totalPopulation = (long)countries.Select<Country, Int64>(c => c.Population).Sum();
            return _totalPopulation;
        }


        public async Task<Int64> GetTotalPopulation(CovidLocation country)
        {
            Int64 population = default;
            try
            {
                population = _getPopulationByCountryFromWWEBScrap(country);

                if (population == default)
                {
                    population = await _getPopulationByCountryFromREST(country);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return population;
            //return _getPopulationByCountryFromREST(country);
            // return Task.FromResult(_getPopulationByCountryFromWWEBScrap(country));
        }

        private string _parseCountryName(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return "";
            if (countryName.ToLower().Contains("congo")) return "Congo";
            else if (countryName.ToLower().Contains("korea") && countryName.ToLower().Contains("south")) return "South Korea";
            else if (countryName.ToLower().Contains("kosovo")) return "Republic of Kosovo";
            else if (countryName.ToLower().Contains("macedonia")) return "North Macedonia";
            return countryName;
        }

        private async Task<int> _getPopulationByCountryFromREST(CovidLocation country)
        {
            try
            {
                string url = "";
                if (!string.IsNullOrEmpty(country.CountryCode))
                {
                    url += $"/alpha/{country.CountryCode.ToLower()}";
                }
                else
                {
                    url += $"/name/{_parseCountryName(country.Country)}?fullText=true";
                }

                var _result = await _httpClient.GetAsync($"{_apiEndpoint}{url}");
                var _content = await _result.Content.ReadAsStringAsync();
                if (_result.IsSuccessStatusCode)
                {
                    try
                    {
                        var _data = JObject.Parse(_content);
                        var _population = _data.Value<int>("population");
                        return _population;
                    }
                    catch
                    {
                        var _data = JArray.Parse(_content);
                        var _population = _data[0].Value<int>("population");
                        return _population;
                    }
                }
                throw new Exception(_content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task _getAllCountriesPopulationFromWebscraping()
        {
            try
            {
                var _result = await _httpClient.GetAsync("https://www.worldometers.info/world-population/population-by-country/");
                if (_result.IsSuccessStatusCode)
                {
                    var _bodyContent = await _result.Content.ReadAsStringAsync();
                    var _document = await _context.OpenAsync(req => req.Content(_bodyContent));
                    var _rowsSelector = "table tbody tr";
                    var _populationCellSelector = "td:nth-child(3)";
                    var _countryNameCellSelector = "td:nth-child(2)";
                    var _rows = _document.QuerySelectorAll(_rowsSelector);
                    foreach (var row in _rows)
                    {
                        var _countryName = row.QuerySelector(_countryNameCellSelector).Text();
                        var _population = row.QuerySelector(_populationCellSelector).Text();
                        //var _population = long.Parse(row.QuerySelector(_populationCellSelector)?.Text);
                        if (string.IsNullOrEmpty(_population)) continue;

                        _CountryPopulationDict.Add(_countryName.ToLower(), long.Parse(_population.Replace(",","")));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private Int64 _getPopulationByCountryFromWWEBScrap(CovidLocation country)
        {

            try
            {
                var _countryName = _parseCountryName(country.Country);
                if (_CountryPopulationDict.ContainsKey(_countryName.ToLower()))
                {
                    return _CountryPopulationDict[_countryName.ToLower()];
                }
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return default;
            }

        }
    }
}

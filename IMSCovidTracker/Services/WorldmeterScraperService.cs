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
        //private IBrowsingContext _context;
        private HttpClient _httpClient;
        private Dictionary<string, string> CountryNameMapper = new Dictionary<string, string>()
        {
            { "UK", "United Kingdom" },
            { "USA", "United States" },
            { "UAE", "United Arab Emirates" },
            { "Palestine", "State of Palestine" },
            { "CAR", "Central African Republic" },
            { "DRC", "DR Congo" },
            { "Saint Kitts and Nevis", "Saint Kitts & Nevis" },
            { "St. Barth", "Saint Barthelemy" },
            { "St. Vincent Grenadines", "St. Vincent & Grenadines" },
            { "S. Korea", "South Korea" }
        };

        public Dictionary<string, Int64> CountryPopulationData = new Dictionary<string, Int64>();

        private Dictionary<string, string> CountryCodeMap = new Dictionary<string, string>();

        public WorldmeterScraperService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            //_context = BrowsingContext.New(Configuration.Default);

            //CreateCountryMap();
        }

        //public void CreateCountryMap()
        //{
        //    Console.WriteLine(String.Format("{0,-30}\t{1}\t{2}\t{3}"
        //                          , "Name"
        //                          , "ISO-2"
        //                          , "ISO-3"
        //                          , "GeoId"));

        //    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        //    {
        //        RegionInfo ri = null;

        //        try
        //        {
        //            ri = new RegionInfo(ci.Name);

        //            CountryCodeMap.Add(ri.EnglishName.ToLower(), ri.TwoLetterISORegionName);
        //            Console.WriteLine(String.Format("{0,-30}\t{1}\t{2}\t{3}"
        //              , ri.EnglishName
        //              , ri.TwoLetterISORegionName
        //              , ri.ThreeLetterISORegionName
        //              , ri.GeoId));
        //        }
        //        catch
        //        {
        //            continue;
        //        }
        //    }
        //}

        public async Task<Int64> ScrapeWorldPopulation()
        {
            // table tbody tr:nth-child(2) th:nth-child(2)
            // https://www.google.com/search?q=world population

            Int64 population = default;
            try
            {
                //var _result = await _httpClient.GetAsync("https://www.worldometers.info/world-population/world-population-by-year/");
                //if (_result.IsSuccessStatusCode)
                //{
                //    var _bodyContent = await _result.Content.ReadAsStringAsync();
                //    var _document = await _context.OpenAsync(req => req.Content(_bodyContent));
                //    var _selector = "table tbody tr:nth-child(1) td:nth-child(2)";
                //    var column = _document.QuerySelector(_selector);
                //    if (column != null)
                //    {
                //        population = Int64.Parse(column.TextContent.Replace(",", ""));
                //    }
                //}
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

        //public async Task ScrapePopulationDataForAllCountries()
        //{
        //    try
        //    {
        //        var _result = await _httpClient.GetAsync("https://www.worldometers.info/world-population/population-by-country/");
        //        if (_result.IsSuccessStatusCode)
        //        {
        //            CountryPopulationData.Clear();

        //            var _bodyContent = await _result.Content.ReadAsStringAsync();
        //            var _document = await _context.OpenAsync(req => req.Content(_bodyContent));
        //            var _rowsSelector = "table tbody tr";
        //            var _populationCellSelector = "td:nth-child(3)";
        //            var _countryNameCellSelector = "td:nth-child(2)";
        //            var _rows = _document.QuerySelectorAll(_rowsSelector);
        //            foreach (var row in _rows)
        //            {
        //                var _countryName = row.QuerySelector(_countryNameCellSelector).Text();
        //                var _population = row.QuerySelector(_populationCellSelector).Text();
        //                //var _population = long.Parse(row.QuerySelector(_populationCellSelector)?.Text);
        //                if (string.IsNullOrEmpty(_population)) continue;

        //                // map wrong country names to match correct names
        //                if (CountryNameMapper.ContainsKey(_countryName)) _countryName = CountryNameMapper[_countryName];

        //                CountryPopulationData.Add(_countryName.ToLower(), long.Parse(_population.Replace(",", "")));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                //var _result = await _httpClient.GetAsync("https://www.worldometers.info/coronavirus/");
                //if (_result.IsSuccessStatusCode)
                //{
                //    var _bodyContent = await _result.Content.ReadAsStringAsync();
                //    var _document = await _context.OpenAsync(req => req.Content(_bodyContent));
                //    var _worldRowCellsSelector = "table#main_table_countries_today tbody tr";

                //    var _rows = _document.QuerySelectorAll(_worldRowCellsSelector);

                //    var _worldRow = _rows.Where(r => r.QuerySelectorAll("td")[0].TextContent.Contains("World"))
                //        .FirstOrDefault();

                //    if (_worldRow != null)
                //    {
                //        var _cells = _worldRow.QuerySelectorAll("td");
                //        _covidLocation = await _cellsToCovidLocation(_cells, true);
                //    }
                //}
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
                //var _result = await _httpClient.GetAsync("https://www.worldometers.info/coronavirus/");
                //if (_result.IsSuccessStatusCode)
                //{
                //    var _bodyContent = await _result.Content.ReadAsStringAsync();
                //    var _document = await _context.OpenAsync(req => req.Content(_bodyContent));
                //    var _worldRowCellsSelector = "table#main_table_countries_today tbody tr";

                //    var _rows = _document.QuerySelectorAll(_worldRowCellsSelector);

                //    foreach(var row in _rows)
                //    {
                //        if (row.GetAttribute("style") != null && row.GetAttribute("style").Contains("display: none")) continue;
                //        if (row.GetAttribute("class") != null && row.GetAttribute("class").Contains("row_continent")) continue;

                //        var _cells = row.QuerySelectorAll("td");
                //        if (_cells != null)
                //        {
                //            _covidLocations.Add(await _cellsToCovidLocation(_cells));
                //        }
                //    }
                //}
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


        /// <summary>
        /// Parse <see cref="IHtmlAllCollection"/> of Row Cells to <see cref="CovidLocation"/>
        /// </summary>
        /// <param name="cells">html cells</param>
        /// <param name="isWorldTotal">if we're getting the world total cases data</param>
        /// <returns></returns>        
        //private async Task<CovidLocation> _cellsToCovidLocation(IHtmlCollection<IElement> cells, bool isWorldTotal=false)
        //{
        //    var _countryName = cells[0].TextContent;
        //    var _confirmed = cells[1].TextContent;
        //    var _deaths = cells[3].TextContent;
        //    var _recovered = cells[5].TextContent;
        //    var _active = cells[6].TextContent;
        //    var _deathsPerMil = cells[9].TextContent;

        //    int.TryParse(_confirmed.Replace(",", ""), out int confirmed);
        //    int.TryParse(_deaths.Replace(",", ""), out int deaths);
        //    int.TryParse(_recovered.Replace(",", ""), out int recovered);
        //    int.TryParse(_active.Replace(",", ""), out int active);
        //    double.TryParse(_deathsPerMil.Replace(",", ""), out double deathsPerMillion);

        //    // map UK > United Kingdom
        //    if (CountryNameMapper.ContainsKey(_countryName)) _countryName = CountryNameMapper[_countryName];

        //    // get total population for country if it exists in dictionary
        //    long _totalPopulation = 0;
        //    if (CountryPopulationData.ContainsKey(_countryName.ToLower()))
        //        _totalPopulation = CountryPopulationData[_countryName.ToLower()];

        //    // for the world, scrape the total population from worldmeter
        //    if (_totalPopulation == default && isWorldTotal)
        //        _totalPopulation = await ScrapeWorldPopulation();

        //    // get the country code for current country if available
        //    string countryCode = "";
        //    if (CountryCodeMap.ContainsKey(_countryName.ToLower()))
        //    {
        //        countryCode = CountryCodeMap[_countryName.ToLower()];
        //    }

        //    return new CovidLocation
        //    {
        //        FlagImageUrl = !string.IsNullOrEmpty(countryCode) ? $"https://www.countryflags.io/{countryCode.ToLower()}/flat/64.png" : "",
        //        CountryCode = countryCode,
        //        Country = _countryName,
        //        Confirmed = confirmed,
        //        Deaths = deaths,
        //        Recovered = recovered,
        //        Active = active,
        //        DeathsPerMillion = deathsPerMillion,
        //        TotalPopulation = _totalPopulation
        //    };
        //}
    }
}

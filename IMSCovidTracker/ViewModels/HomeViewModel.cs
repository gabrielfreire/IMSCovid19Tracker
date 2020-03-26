using IMSCovidTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMSCovidTracker.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private members
        private CovidLocation _totalCases;
        private CovidLocation _ireland;
        private CovidLocation _uk;
        private CovidLocation _brazil;
        private CovidLocation _italy;
        private CovidLocation _china;
        private CovidLocation _romania;
        private CovidLocation _usa;
        private CovidLocation _resultCountry;
        private IEnumerable<CovidLocation> _covidLocations = new List<CovidLocation>();
        private string _searchQuery;
        private bool _searchSuccess = false;
        #endregion

        #region Public members
        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public IEnumerable<CovidLocation> CovidLocations { get => _covidLocations; set => RaiseIfPropertyChanged(ref _covidLocations, value); }
        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        public CovidLocation TotalCases { get => _totalCases; set => RaiseIfPropertyChanged(ref _totalCases, value); }
        public CovidLocation Ireland { get => _ireland; set => RaiseIfPropertyChanged(ref _ireland, value); }


        public CovidLocation Uk { get => _uk; set => RaiseIfPropertyChanged(ref _uk, value); }
        public CovidLocation Brazil { get => _brazil; set => RaiseIfPropertyChanged(ref _brazil, value); }
        public CovidLocation Italy { get => _italy; set => RaiseIfPropertyChanged(ref _italy, value); }
        public CovidLocation China { get => _china; set => RaiseIfPropertyChanged(ref _china, value); }
        public CovidLocation Romania { get => _romania; set => RaiseIfPropertyChanged(ref _romania, value); }
        public CovidLocation USA { get => _usa; set => RaiseIfPropertyChanged(ref _usa, value); }
        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }

        #endregion


        #region Default constructor
        public HomeViewModel() { }
        #endregion

        public async Task LoadCovidData()
        {
            try
            {
                SetBusy(true);
                CovidLocations = await App.CovidService.GetLocationsAsync();
                TotalCases = App.CovidService.GetTotalCases(CovidLocations);
                Ireland = App.CovidService.Find("Ireland");
                Uk = App.CovidService.Find("United Kingdom");
                Brazil = App.CovidService.Find("Brazil");
                Italy = App.CovidService.Find("Italy");
                China = App.CovidService.Find("China");
                Romania = App.CovidService.Find("Romania");
                USA = App.CovidService.Find("us");
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

    }

}

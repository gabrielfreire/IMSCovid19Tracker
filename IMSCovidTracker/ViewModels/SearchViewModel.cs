using IMSCovidTracker.Models;
using System;

namespace IMSCovidTracker.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {

        private CovidLocation _resultCountry;
        private string _searchQuery;
        private bool _searchSuccess = false;


        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }


        public void Search()
        {
            try
            {

                SearchSuccess = false;
                SetBusy(true);
                if (string.IsNullOrEmpty(SearchQuery))
                {
                    App.MessageDialogService.Display("Error", "Please provide a search query");
                    return;
                }

                var _resultCountry = App.CovidService.Find(SearchQuery);
                
                if (_resultCountry == null)
                {
                    App.MessageDialogService.Display("Error", $"Country {SearchQuery} not found");
                    return;
                }
                ResultCountry = _resultCountry;
                SearchSuccess = true;
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", $"Something weird has happened: {ex.Message}");

            }
            finally
            {
                SetBusy(false);
            }

        }
    }
}

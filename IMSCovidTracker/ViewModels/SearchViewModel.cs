using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {

        private CovidLocation _resultCountry;
        private ObservableCollection<CovidLocation> _searchPartialResults = new ObservableCollection<CovidLocation>();
        private string _searchQuery;
        private bool _searchSuccess = false;
        private bool _isSearching = false;
        private SearchPage searchPage;


        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }
        public ObservableCollection<CovidLocation> SearchPartialResults { get => _searchPartialResults; set => RaiseIfPropertyChanged(ref _searchPartialResults, value); }
        public bool IsSearching { get => _isSearching; set => RaiseIfPropertyChanged(ref _isSearching, value); }

        public ICommand SelectCountryCommand => new Command<string>(async (countryName) => await SearchCommand(countryName));


        public SearchViewModel(SearchPage searchPage)
        {
            this.searchPage = searchPage;
        }

        private Task SearchCommand(string countryName)
        {
            searchPage.SearchField.TextChanged -= searchPage.SearchField_TextChanged;
            Device.BeginInvokeOnMainThread(() => {
                ResultCountry = null;
                SearchSuccess = false;
                SearchPartialResults.Clear();
                IsSearching = false;
            });
            SearchQuery = countryName;
            return Task.CompletedTask;
        }

        public void Search()
        {
            try
            {
                searchPage.SearchField.TextChanged -= searchPage.SearchField_TextChanged;
                searchPage.SearchField.TextChanged += searchPage.SearchField_TextChanged;

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

        public void SearchPartial()
        {
            try
            {

                var _results = App.CovidService.FindPartial(SearchQuery);

                Device.BeginInvokeOnMainThread(() =>
                {
                    IsSearching = true;
                    SearchPartialResults.Clear();
                    foreach ( var result in _results )
                    {
                        if (SearchPartialResults.FirstOrDefault((r) => r.Country == result.Country) != null) return;
                        SearchPartialResults.Add(result);
                    }
                });
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", $"Something weird has happened: {ex.Message}");   
            }
        }
    }
}

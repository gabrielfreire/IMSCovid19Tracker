using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class SearchModalViewModel : BaseViewModel
    {
        private bool _isSearching = false;
        private string _searchQuery;
        private SearchModalPage _searchModalPage;

        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        private ObservableCollection<string> _searchPartialResults = new ObservableCollection<string>();


        public ObservableCollection<string> SearchPartialResults { get => _searchPartialResults; set => RaiseIfPropertyChanged(ref _searchPartialResults, value); }
        public bool IsSearching { get => _isSearching; set => RaiseIfPropertyChanged(ref _isSearching, value); }
        public ICommand SelectCountryCommand => new Command<string>(async (countryName) => await SearchCommand(countryName));
        public ICommand SendResultCommand => new Command(() => SendResult());

        private void SendResult()
        {
            MessagingCenter.Send<SearchModalViewModel, string>(this, "receivedCountryName", SearchQuery);
            _ = App.NavigationService.NavigateBack(_searchModalPage, true);
        }

        public SearchModalViewModel(SearchModalPage searchModalPage)
        {
            _searchModalPage = searchModalPage;
        }

        private Task SearchCommand(string countryName)
        {
            _searchModalPage.SearchField.TextChanged -= _searchModalPage.SearchField_TextChanged;
            Device.BeginInvokeOnMainThread(() => {
                SearchPartialResults.Clear();
                IsSearching = false;

                _searchModalPage.SearchField.TextChanged += _searchModalPage.SearchField_TextChanged;
            });
            SearchQuery = countryName;


            return Task.CompletedTask;
        }

        public void SearchPartial()
        {
            try
            {
                var _results = App.CovidService.FindPartial(SearchQuery);

                Device.BeginInvokeOnMainThread(async() =>
                {
                    IsSearching = true;
                    SearchPartialResults.Clear();
                    
                    await Task.Delay(10);

                    if (_results == null) return;

                    foreach (var result in _results)
                    {
                        if (SearchPartialResults.Contains(result.Country)) continue;
                        SearchPartialResults.Add(result.Country);
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

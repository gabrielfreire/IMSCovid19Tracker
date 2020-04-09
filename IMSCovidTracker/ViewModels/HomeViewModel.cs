using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private members
        private CovidLocation _totalCases;
        private CovidLocation _resultCountry;
        private ObservableCollection<CovidLocation> _countryWidgets = new ObservableCollection<CovidLocation>();
        private string _searchQuery;
        private bool _searchSuccess = false;
        private HomePage _homePage;
        private int _widgetsCount => CountryWidgets.Count; 
        #endregion

        #region Public members
        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public ObservableCollection<CovidLocation> CountryWidgets { get => _countryWidgets; set => RaiseIfPropertyChanged(ref _countryWidgets, value); }
        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        public CovidLocation TotalCases { get => _totalCases; set => RaiseIfPropertyChanged(ref _totalCases, value); }

        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }

        public ICommand DeleteWidgetCommand => new Command<CovidLocation>((cLoc) => DeleteWidget(cLoc));
        public ICommand AddWidgetCommand => new Command(async () => await AddWidget());

        
        #endregion


        #region Default constructor

        public HomeViewModel(HomePage homePage)
        {
            _homePage = homePage;
        }
        #endregion

        public async Task LoadCovidData()
        {
            try
            {
                SetBusy(true);
                var _covidLocations = await App.CovidService.GetLocationsAsync();
                TotalCases = App.CovidService.GetTotalCases(_covidLocations);
                
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

        public async Task LoadDefaultWidgets()
        {
            try
            {
                SetBusy(true);

                var _storedWidgets = await App.StorageService.GetWidgetPreferences();
                if (_storedWidgets == null)
                {

                    CountryWidgets.Add(App.CovidService.Find("Ireland"));
                    CountryWidgets.Add(App.CovidService.Find("United Kingdom"));
                    CountryWidgets.Add(App.CovidService.Find("Brazil"));
                    CountryWidgets.Add(App.CovidService.Find("Italy"));
                    CountryWidgets.Add(App.CovidService.Find("Romania"));
                    CountryWidgets.Add(App.CovidService.Find("us"));
                }
                else
                {
                    CountryWidgets = _storedWidgets;
                }

            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.ToString());
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async Task AddWidget()
        {
            if (CountryWidgets.Count >= 6)
            {
                App.MessageDialogService.Display("Maximum number reached", "Reached the maximum number of Widgets");
                return;
            }

            MessagingCenter.Unsubscribe<SearchModalViewModel, string>(this, "receivedCountryName");
            MessagingCenter.Subscribe<SearchModalViewModel, string>(this, "receivedCountryName", AddCountryWidget);
            await App.NavigationService.Navigate(_homePage, new SearchModalPage(), true);
        }

        private void AddCountryWidget(SearchModalViewModel sender, string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return;

            var _country = App.CovidService.Find(countryName);

            if (_country == null)
            {
                App.MessageDialogService.Display("Not found", $"Country {countryName} not found");
                return;
            }



            MessagingCenter.Unsubscribe<SearchModalViewModel, string>(this, "receivedCountryName");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);

                CountryWidgets.Add(_country);
                await App.StorageService.StoreWidgetPreferences(CountryWidgets);
                _homePage.ForceLayout();
            });
        }

        private void DeleteWidget(CovidLocation cLoc)
        {
            try
            {
                CountryWidgets.Remove(cLoc);
                _ = App.StorageService.StoreWidgetPreferences(CountryWidgets);
                
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.ToString());
            }
            finally
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(500);
                    _homePage.ForceLayout();
                });
            }
        }

    }

}

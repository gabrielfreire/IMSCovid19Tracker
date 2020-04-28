using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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
        public ICommand AddWidgetCommand => new Command(async () => await OpenAddWidgetModal());
        public ICommand ViewWidgetCommand => new Command<CovidLocation>(async (loc) => await ViewWidget(loc));

        #endregion


        #region Default constructor

        public HomeViewModel(HomePage homePage)
        {
            _homePage = homePage;
        }
        #endregion

        

        public async Task LoadCovidData(bool isRefresh=false)
        {
            try
            {
                SetBusy(true);

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    App.MessageDialogService.Display("No Connection", "Sorry, you're not connected to the internet.");
                    return;
                }

                // only reload data if user is refreshing the page
                if (isRefresh)
                    _ = App.LoadData();

                while (!App.DataLoaded) await Task.Delay(10);

                // Get total covid cases in the world
                TotalCases = await App.CovidService.GetTotalCases();
                TotalCases.FlagImageUrl = "world.png";

                await LoadDefaultWidgets();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                App.MessageDialogService.Display("Error", "Failed to load COVID-19 Data, check your internet connection.");
            }
            finally
            {
                SetBusy(false);
            }
        }

        public async Task LoadDefaultWidgets()
        {
            ObservableCollection<CovidLocation> _storedWidgets = new ObservableCollection< CovidLocation>();
            IEnumerable<string> _storedCountries = new List<string>();
            try
            {
                SetBusy(true);

                if (!App.CovidService.CovidLocations.Any())
                {
                    App.MessageDialogService.Display("Error", "Failed to load widgets, check your internet connection");
                    return;
                }

                _storedCountries = await App.StorageService.GetWidgetPreferences();
                
                if (_storedCountries != null)
                {
                    foreach (var country in _storedCountries)
                    {
                        var _countryLocation = App.CovidService.Find(country);
                        if (_countryLocation != null)
                        {
                            _storedWidgets.Add(_countryLocation);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                App.MessageDialogService.Display("Error", ex.ToString());
            }
            finally
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    CountryWidgets.Clear();

                    await Task.Delay(10);

                    if (_storedWidgets.Count == 0)
                    {
                        CountryWidgets.Add(App.CovidService.Find("Ireland"));
                        CountryWidgets.Add(App.CovidService.Find("united kingdom"));
                        CountryWidgets.Add(App.CovidService.Find("Brazil"));
                        CountryWidgets.Add(App.CovidService.Find("Italy"));
                        CountryWidgets.Add(App.CovidService.Find("Romania"));
                        CountryWidgets.Add(App.CovidService.Find("united states"));
                    }
                    else
                    {
                        CountryWidgets = _storedWidgets;
                    }


                    _ = ForceCollectionLayout(0);

                });
                SetBusy(false);
            }
        }

        private async Task OpenAddWidgetModal()
        {
            if (CountryWidgets.Count >= 6)
            {
                App.MessageDialogService.Display("Maximum number reached", "You can only have 6 widgets.");
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
                await App.StorageService.StoreWidgetPreferences(CountryWidgets.Select(c => c.Country).ToList());

                // force layout of collectionview
                _ = ForceCollectionLayout();
            });
        }

        private void DeleteWidget(CovidLocation cLoc)
        {
            try
            {
                if (!CountryWidgets.Any()) return;

                CountryWidgets.Remove(cLoc);
                _ = App.StorageService.StoreWidgetPreferences(CountryWidgets.Select(c => c.Country).ToList());

                // force layout of collectionview
                _ = ForceCollectionLayout();

            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.ToString());
            }
        }

        private async Task ViewWidget(CovidLocation location)
        {
            await App.NavigationService.Navigate(_homePage, new ViewWidgetPage(location), false);
        }

        /// <summary>
        /// Force layout for collectionview
        /// </summary>
        /// <returns></returns>
        private async Task ForceCollectionLayout(int delay=400)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                _homePage.ForceLayout();
                return;
            }
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(delay);
                _homePage.WidgetCollection.ItemsSource = null;
                _homePage.WidgetCollection.ItemsSource = CountryWidgets;

                _homePage.WidgetCollection.HeightRequest = 3 * 130;
            });
        }
    }

}

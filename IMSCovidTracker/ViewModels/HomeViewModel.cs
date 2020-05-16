using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private members
        private CovidLocation _totalCases;
        private CovidLocation _resultCountry;
        private string _searchQuery;
        private bool _searchSuccess = false;
        #endregion

        #region Public members

        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public ObservableCollection<CovidLocation> CountryWidgets { get; } = new ObservableCollection<CovidLocation>();
        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }
        public CovidLocation TotalCases { get => _totalCases; set => RaiseIfPropertyChanged(ref _totalCases, value); }

        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }

        public ICommand DeleteWidgetCommand => new Command<CovidLocation>((cLoc) => DeleteWidget(cLoc));
        public ICommand AddWidgetCommand => new Command(async () => await OpenAddWidgetModal());
        public ICommand ViewWidgetCommand => new Command<CovidLocation>(async (loc) => await ViewWidget(loc));
        public ICommand RefreshDataCommand => new Command(async () => await RefreshData());


        #endregion


        #region Default constructor

        public HomeViewModel()
        {
            RefreshDataCommand.Execute(null);
        }
        #endregion


        private async Task RefreshData()
        {
            await LoadCovidData(true);
        }

        public async Task LoadCovidData(bool isRefresh = false)
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
                    await App.LoadData();


                // Get total covid cases in the world
                var _totalCase = await App.CovidService.GetTotalCases();
                _totalCase.FlagImageUrl = "world.png";

                Device.BeginInvokeOnMainThread(() =>
                {
                    TotalCases = _totalCase;
                });

                await LoadDefaultWidgets();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                App.MessageDialogService.Display("Connection Error", "Failed to load COVID-19 Data, check your internet connection and refresh.");
            }
            finally
            {
                SetBusy(false);
            }
        }

        public async Task LoadDefaultWidgets()
        {
            ObservableCollection<CovidLocation> _storedWidgets = new ObservableCollection<CovidLocation>();
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
                else
                {
                    // no widget config found
                    // add default widgets
                    _storedWidgets.Add(App.CovidService.Find("Ireland"));
                    _storedWidgets.Add(App.CovidService.Find("united kingdom"));
                    _storedWidgets.Add(App.CovidService.Find("Brazil"));
                    _storedWidgets.Add(App.CovidService.Find("Italy"));
                    _storedWidgets.Add(App.CovidService.Find("Romania"));
                    _storedWidgets.Add(App.CovidService.Find("united states"));
                
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                App.MessageDialogService.Display("Error", "Something wrong happened");
            }
            finally
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    CountryWidgets.Clear();

                    await Task.Delay(10);

                    foreach (var item in _storedWidgets)
                    {
                        CountryWidgets.Add(item);
                    }
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

            await App.NavigationService.Navigate(new SearchModalPage(), true);
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

            });
        }

        private void DeleteWidget(CovidLocation cLoc)
        {
            try
            {
                if (!CountryWidgets.Any()) return;

                CountryWidgets.Remove(cLoc);

                _ = App.StorageService.StoreWidgetPreferences(CountryWidgets.Select(c => c.Country).ToList());
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.ToString());
            }
        }

        private async Task ViewWidget(CovidLocation location)
        {
            await App.NavigationService.Navigate(new ViewWidgetPage(location), true);
        }
    }

}

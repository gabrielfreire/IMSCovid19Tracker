using IMSCovidTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CovidLocationSearchBox : ContentView, INotifyPropertyChanged
    {
        private string _searchQuery = "";
        private bool _isSearching = false;
        private ObservableCollection<string> _searchPartialResults = new ObservableCollection<string>();


        public static readonly BindableProperty OnCountrySelectedProperty = BindableProperty.Create(nameof(OnCountrySelected), typeof(ICommand), typeof(CovidLocationSearchBox),null);
        public ICommand OnCountrySelected { get => (ICommand)GetValue(OnCountrySelectedProperty); set => SetValue(OnCountrySelectedProperty, value); }

        public bool IsSearching { get => _isSearching; set => RaiseIfPropertyChanged(ref _isSearching, value); }

        public string SearchQuery { get => _searchQuery; set => RaiseIfPropertyChanged(ref _searchQuery, value); }

        public ObservableCollection<string> SearchPartialResults { get => _searchPartialResults; set => RaiseIfPropertyChanged(ref _searchPartialResults, value); }

        public ICommand SelectCountryCommand => new Command<string>((countryName) => SelectCountry(countryName));

        
        public CovidLocationSearchBox()
        {
            BindingContext = this;

            InitializeComponent();

            Clear();

            SearchField.TextChanged -= SearchField_TextChanged;
            SearchField.TextChanged += SearchField_TextChanged;
        }

        public void Clear()
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                SearchQuery = "";
                IsSearching = false;
                SearchPartialResults.Clear();
            });
        }

        private void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                Clear();
            } 
            else
            {
                AutocompleteSearch();
            }
        }

        private void AutocompleteSearch()
        {
            try
            {
                var _results = App.CovidService.FindPartial(SearchQuery);

                Device.BeginInvokeOnMainThread(async () =>
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

        private void SelectCountry(string countryName)
        {
            SearchField.TextChanged -= SearchField_TextChanged;

            Device.BeginInvokeOnMainThread(async () => {
                SearchQuery = countryName;
                SearchPartialResults.Clear();

                await Task.Delay(10);

                IsSearching = false;
                SearchField.TextChanged += SearchField_TextChanged;
            });

            OnCountrySelected?.Execute(countryName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaiseIfPropertyChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return;

            property = value;
            OnPropertyChanged(propertyName);
        }

    }
}
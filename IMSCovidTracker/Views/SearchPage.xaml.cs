using IMSCovidTracker.Components;
using IMSCovidTracker.Models;
using IMSCovidTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private SearchViewModel _viewModel;
        
        public SearchPage()
        {
            BindingContext = _viewModel = new SearchViewModel(this);
            InitializeComponent();
        }

        private void SearchField_OnCountrySelected(string countryName)
        {
            _viewModel.ResetSearch();
            _viewModel.DisplaySearchResult(countryName);
        }

        protected override void OnAppearing()
        {
            SearchField.Clear();
            SearchField.OnCountrySelected -= SearchField_OnCountrySelected;
            SearchField.OnCountrySelected += SearchField_OnCountrySelected;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.ResetSearch();
            SearchField.Clear();
            base.OnDisappearing();
        }
    }
}
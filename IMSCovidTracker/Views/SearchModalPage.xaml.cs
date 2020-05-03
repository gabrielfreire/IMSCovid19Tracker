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
    public partial class SearchModalPage : ContentPage
    {
        private SearchModalViewModel _viewModel;

        public SearchModalPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SearchModalViewModel();
        }

        protected override void OnAppearing()
        {
            locationSearchField.Clear();
            locationSearchField.OnCountrySelected -= LocationSearchField_OnCountrySelected;
            locationSearchField.OnCountrySelected += LocationSearchField_OnCountrySelected;
            base.OnAppearing();
        }

        private void LocationSearchField_OnCountrySelected(string countryName)
        {
            _viewModel.SendResult(countryName);
        }
    }
}
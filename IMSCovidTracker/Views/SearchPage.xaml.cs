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
            SearchField.TextChanged -= SearchField_TextChanged;
            SearchField.TextChanged += SearchField_TextChanged;
        }

        public void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
           _viewModel.SearchPartial();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.Search();
        }

        protected override void OnAppearing()
        {
            SearchField.TextChanged -= SearchField_TextChanged;
            SearchField.TextChanged += SearchField_TextChanged;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.ResetSearch(resetQuery: true);
            base.OnDisappearing();
        }
    }
}
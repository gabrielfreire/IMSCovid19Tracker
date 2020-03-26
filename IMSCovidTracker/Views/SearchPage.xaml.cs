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
            BindingContext = _viewModel = new SearchViewModel();
            InitializeComponent();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.Search();
        }
    }
}
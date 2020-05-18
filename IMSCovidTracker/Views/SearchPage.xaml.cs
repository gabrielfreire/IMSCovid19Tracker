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
        
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            SearchField.Clear();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((SearchViewModel)BindingContext).ResetSearch();
            SearchField.Clear();
            base.OnDisappearing();
        }
    }
}
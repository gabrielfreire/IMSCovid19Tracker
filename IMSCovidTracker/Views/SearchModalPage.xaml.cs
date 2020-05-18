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

        public SearchModalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            locationSearchField.Clear();
            base.OnAppearing();
        }
    }
}
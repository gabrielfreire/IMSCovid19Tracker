using IMSCovidTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HomeViewModel(this);
            // where to display tutorial
            navbarComponent.AbsoluteLayoutElement = homeLayout;
        }

        protected override void OnAppearing()
        {
            

            base.OnAppearing();
            
            // CollectionView in iOS is a bit buggy but forcing layout after rendering seems to solve some issues
            //if (Device.RuntimePlatform == Device.iOS)
            ForceLayout();

            // show only in first launch
            if (VersionTracking.IsFirstLaunchEver)
            {
                _ = navbarComponent.ShowTutorial();
            }
        }


    }
}
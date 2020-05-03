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
            BindingContext = _viewModel = new HomeViewModel(this);
            InitializeComponent();

            // where to display tutorial
            navbarComponent.AbsoluteLayoutElement = homeLayout;

            RefreshData(false, true);
        }


        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            RefreshData(true, false);
        }

        private void RefreshData(bool IsRefresh, bool displayTutorialAfterRefresh=false)
        {
            _ = Task.Run(async () =>
            {
                await _viewModel.LoadCovidData(IsRefresh);

                // show only in first launch
                if (displayTutorialAfterRefresh && VersionTracking.IsFirstLaunchEver)
                {
                    await navbarComponent.ShowTutorial();
                }
            });
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


    }
}
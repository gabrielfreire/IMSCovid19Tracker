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

            BindingContext = _viewModel = new HomeViewModel();

            // where to display tutorial
            navbarComponent.AbsoluteLayoutElement = homeLayout;

            _viewModel.WidgetCollectionChanged += _viewModel_WidgetCollectionChanged;

        }

        private void _viewModel_WidgetCollectionChanged(object sender, int delay)
        {
            _ = ForceCollectionLayout(delay);
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

        /// <summary>
        /// <i>CollectionLayout is a bit buggy when adding/removing items so we must force some kind of re-layout</i> <br/>
        /// Force layout for collectionview
        /// </summary>
        /// <returns></returns>
        private async Task ForceCollectionLayout(int delay = 400)
        {
            

            await Device.InvokeOnMainThreadAsync(async () =>
            {

                WidgetCollection.HeightRequest = 3 * 145;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    ForceLayout();
                    return;
                }

                await Task.Delay(delay);
                WidgetCollection.ItemsSource = null;
                WidgetCollection.ItemsSource = _viewModel.CountryWidgets;

            });
            
        }
    }
}
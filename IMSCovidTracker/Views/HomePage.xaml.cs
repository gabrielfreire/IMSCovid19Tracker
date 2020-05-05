using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            // where to display tutorial
            navbarComponent.AbsoluteLayoutElement = homeLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // show only in first launch
            if (VersionTracking.IsFirstLaunchEver)
            {
                _ = navbarComponent.ShowTutorial();
            }
        }
    }
}
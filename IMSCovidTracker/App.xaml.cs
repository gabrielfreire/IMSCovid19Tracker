using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IMSCovidTracker.Views;
using IMSCovidTracker.Services;
using System.Threading.Tasks;

namespace IMSCovidTracker
{
    public partial class App : Application
    {
        public static bool DataLoaded = false;
        public static bool AppLaunchedFirstTime = false;
        public static MessageDialogService MessageDialogService { get; set; }
        public static CovidService CovidService { get; set; }
        public static WorldmeterScraperService WorldmeterScraperService { get; set; }
        public static NavigationService NavigationService{ get; set; }
        public static StorageService StorageService { get; set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MessageDialogService>();
            DependencyService.Register<CovidService>();
            DependencyService.Register<NavigationService>();
            DependencyService.Register<StorageService>();
            DependencyService.Register<WorldmeterScraperService>();

            MessageDialogService = DependencyService.Get<MessageDialogService>();
            CovidService = DependencyService.Get<CovidService>();
            NavigationService = DependencyService.Get<NavigationService>();
            StorageService = DependencyService.Get<StorageService>();
            WorldmeterScraperService = DependencyService.Get<WorldmeterScraperService>();

            _ = LoadData();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static async Task LoadData()
        {
            try
            {
                DataLoaded = false;

                var _firstLoadState = await StorageService.GetFirstLaunchState();
                if (_firstLoadState != null)
                {
                    AppLaunchedFirstTime = _firstLoadState.Value;
                }
                else
                {
                    // we store information that user launched the app for the first time
                    await StorageService.SetFirstLauncState(new Models.FirstLaunchState { Value = true });
                    
                    // and we treat this as first load
                    AppLaunchedFirstTime = false;
                }

                // Get all countries covid cases
                await App.CovidService.GetLocationsAsync();

                DataLoaded = true;
            }
            catch (Exception ex)
            {
                DataLoaded = false;
                MessageDialogService.Display("Error", "Could not get latest informations! check your internet connection.");
            }
        }
    }
}

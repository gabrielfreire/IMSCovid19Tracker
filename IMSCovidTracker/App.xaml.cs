using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IMSCovidTracker.Views;
using IMSCovidTracker.Services;

namespace IMSCovidTracker
{
    public partial class App : Application
    {
        public static MessageDialogService MessageDialogService { get; set; }
        public static CovidService CovidService { get; set; }
        public static NavigationService NavigationService{ get; set; }
        public static StorageService StorageService { get; set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MessageDialogService>();
            DependencyService.Register<CovidService>();
            DependencyService.Register<NavigationService>();
            DependencyService.Register<StorageService>();

            MessageDialogService = DependencyService.Get<MessageDialogService>();
            CovidService = DependencyService.Get<CovidService>();
            NavigationService = DependencyService.Get<NavigationService>();
            StorageService = DependencyService.Get<StorageService>();

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
    }
}

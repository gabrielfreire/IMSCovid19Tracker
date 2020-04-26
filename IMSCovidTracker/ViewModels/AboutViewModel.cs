using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _currentVersion;
        public string CurrentVersion { get => _currentVersion; set => RaiseIfPropertyChanged(ref _currentVersion, value); }

        public ICommand OpenWebsiteCommand => new Command(async () => await OpenWebsite());

        public AboutViewModel()
        {
            _currentVersion = GetCurrentVersion();
        }

        private string GetCurrentVersion()
        {
            if (Device.RuntimePlatform == Device.UWP) return "2.0.5";
            return VersionTracking.CurrentVersion;
        }


        private async Task OpenWebsite()
        {
            await Launcher.OpenAsync("https://gabrielfreire.github.io");
        }
    }
}
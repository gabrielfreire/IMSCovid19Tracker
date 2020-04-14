using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _currentVersion;
        public string CurrentVersion { get => _currentVersion; set => RaiseIfPropertyChanged(ref _currentVersion, value); }
        public AboutViewModel()
        {
            _currentVersion = GetCurrentVersion();
        }

        private string GetCurrentVersion()
        {
            if (Device.RuntimePlatform == Device.UWP) return "2.0.2";
            return VersionTracking.CurrentVersion;
        }

    }
}
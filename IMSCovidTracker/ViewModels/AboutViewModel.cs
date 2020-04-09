using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _currentVersion = VersionTracking.CurrentVersion;
        public string CurrentVersion { get => _currentVersion; set => RaiseIfPropertyChanged(ref _currentVersion, value); }
        public AboutViewModel()
        {
        }

    }
}
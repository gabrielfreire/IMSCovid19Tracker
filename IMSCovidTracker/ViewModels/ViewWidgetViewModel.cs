using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    class ViewWidgetViewModel : BaseViewModel
    {
        private CovidLocation _countryWidget;
        public CovidLocation CountryWidget { get => _countryWidget; set => RaiseIfPropertyChanged(ref _countryWidget, value); }

        public ICommand CloseModalCommand => new Command(async() => await CloseModal());

        public ViewWidgetPage ViewWidgetPage { get; }

        public ViewWidgetViewModel(Views.ViewWidgetPage viewWidgetPage, CovidLocation country)
        {
            Init(country);
            ViewWidgetPage = viewWidgetPage;
        }

        private async Task CloseModal()
        {
            await App.NavigationService.NavigateBack(ViewWidgetPage, true);
        }

        public void Init(CovidLocation country)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                CountryWidget = country;
            });

        }
    }
}

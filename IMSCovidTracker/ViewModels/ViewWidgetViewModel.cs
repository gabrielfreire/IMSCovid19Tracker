using IMSCovidTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMSCovidTracker.ViewModels
{
    class ViewWidgetViewModel : BaseViewModel
    {
        private CovidLocation _countryWidget;
        public CovidLocation CountryWidget { get => _countryWidget; set => RaiseIfPropertyChanged(ref _countryWidget, value); }
        public ViewWidgetViewModel(CovidLocation country)
        {
            _ = Init(country);
        }

        public async Task Init(CovidLocation country)
        {
            try
            {
                SetBusy(true);

                country.TotalPopulation = await App.CountryService.GetTotalPopulation(country);
                var deathsPerPop = (double)country.Deaths / country.TotalPopulation;
                country.DeathsPerMillion = (int)(deathsPerPop * 1000000d);
            }
            catch
            {
                // App.MessageDialogService.Display("Population", "Failed to get total population");
            }
            finally
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    CountryWidget = country;
                });
                SetBusy(false);
            }

        }
    }
}

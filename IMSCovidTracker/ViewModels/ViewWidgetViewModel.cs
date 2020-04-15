using IMSCovidTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMSCovidTracker.ViewModels
{
    class ViewWidgetViewModel : BaseViewModel
    {
        private CovidLocation _countryWidget;
        public CovidLocation CountryWidget { get => _countryWidget; set => RaiseIfPropertyChanged(ref _countryWidget, value); }
        public ViewWidgetViewModel(CovidLocation countryWidget)
        {
            CountryWidget = countryWidget;
        }
    }
}

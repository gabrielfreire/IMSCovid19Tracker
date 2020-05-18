using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class SearchModalViewModel : BaseViewModel
    {
        public Command<string> CountrySelectedCommand => new Command<string>(async (cName) => await HandleCountrySelected(cName));

        private Task HandleCountrySelected(string cName)
        {
            SendResult(cName);
            return Task.CompletedTask;
        }

        public void SendResult(string countryName)
        {
            MessagingCenter.Send<SearchModalViewModel, string>(this, "receivedCountryName", countryName);
            _ = App.NavigationService.NavigateBack(true);
        }

    }
}

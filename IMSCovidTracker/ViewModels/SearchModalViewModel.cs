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
        private SearchModalPage _searchModalPage;


        public SearchModalViewModel(SearchModalPage searchModalPage)
        {
            _searchModalPage = searchModalPage;
        }

        public void SendResult(string countryName)
        {
            MessagingCenter.Send<SearchModalViewModel, string>(this, "receivedCountryName", countryName);
            _ = App.NavigationService.NavigateBack(_searchModalPage, true);
        }

    }
}

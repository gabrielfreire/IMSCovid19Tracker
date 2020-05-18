﻿using IMSCovidTracker.Models;
using IMSCovidTracker.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMSCovidTracker.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {

        private CovidLocation _resultCountry;
        private bool _searchSuccess = false;

        public bool SearchSuccess { get => _searchSuccess; set => RaiseIfPropertyChanged(ref _searchSuccess, value); }
        public CovidLocation ResultCountry { get => _resultCountry; set => RaiseIfPropertyChanged(ref _resultCountry, value); }

        public ICommand CountrySelectedCommand => new Command<string>(async (cName) => await HandleCountrySelected(cName));

        private Task HandleCountrySelected(string cName)
        {
            ResetSearch();
            DisplaySearchResult(cName);
            return Task.CompletedTask;
        }

        public void DisplaySearchResult(string countryName)
        {
            try
            {
                SearchSuccess = false;
                SetBusy(true);
                if (string.IsNullOrEmpty(countryName))
                {
                    App.MessageDialogService.Display("Error", "Please provide a search query");
                    return;
                }

                var _resultCountry = App.CovidService.Find(countryName);

                if (_resultCountry == null)
                {
                    App.MessageDialogService.Display("Error", $"Country {countryName} not found");
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    ResultCountry = _resultCountry;
                    SearchSuccess = true;
                });
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", $"Something weird has happened: {ex.Message}");

            }
            finally
            {
                SetBusy(false);
            }

        }

        public void ResetSearch()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ResultCountry = null;
                SearchSuccess = false;
            });
        }
    }
}

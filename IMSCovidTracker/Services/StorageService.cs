using IMSCovidTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IMSCovidTracker.Services
{
    public class StorageService
    {
        public static string WIDGET_KEY = "widget_preferences";
        public async Task StoreWidgetPreferences(ObservableCollection<CovidLocation> value)
        {
            try
            {
                var _strValue = JsonConvert.SerializeObject(value);
                await SecureStorage.SetAsync(WIDGET_KEY, _strValue);
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.Message);
            }
        }

        public async Task<ObservableCollection<CovidLocation>> GetWidgetPreferences()
        {
            try
            {
                var _strValue = await SecureStorage.GetAsync(WIDGET_KEY);
                if (_strValue == null) return null;
                return JsonConvert.DeserializeObject<ObservableCollection<CovidLocation>>(_strValue);
            }
            catch (Exception ex)
            {
                App.MessageDialogService.Display("Error", ex.Message);
                return null;
            }
        }
    }
}

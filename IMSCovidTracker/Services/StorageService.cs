using IMSCovidTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMSCovidTracker.Services
{
    public class StorageService
    {
        public static string WIDGET_KEY = "widget_preferences";

        public async Task StoreWidgetPreferences(IEnumerable<string> value)
        {
            if (Device.RuntimePlatform == Device.UWP) return;
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

        public async Task<IEnumerable<string>> GetWidgetPreferences()
        {
            if (Device.RuntimePlatform == Device.UWP) return null;
            try
            {
                var _strValue = await SecureStorage.GetAsync(WIDGET_KEY);
                if (_strValue == null) return null;
                return JsonConvert.DeserializeObject<IEnumerable<string>>(_strValue);
            }
            catch (Exception ex)
            {
                SecureStorage.Remove(WIDGET_KEY);
                App.MessageDialogService.Display("Error", ex.Message);
                return null;
            }
        }
    }
}

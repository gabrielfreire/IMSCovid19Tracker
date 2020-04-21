using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMSCovidTracker.Services
{
    public class MessageDialogService
    {

        public void Display(string Title, string Message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert(Title, Message, "Ok");
            });
        }

        public async Task DisplayAsync(string Title, string Message)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await App.Current.MainPage.DisplayAlert(Title, Message, "Ok");
            });
        }

        public Task<string> PromptUser(string Title, string Message)
        {
            return App.Current.MainPage.DisplayPromptAsync(Title, Message);
        }

        public async Task DisplayTutorial(Frame countryWidgetInfo, int displayTime = 4000)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(500);
                await countryWidgetInfo.FadeTo(0.8, 300);
                await Task.Delay(displayTime);
                await countryWidgetInfo.FadeTo(0, 300);
            });
        }
    }
}

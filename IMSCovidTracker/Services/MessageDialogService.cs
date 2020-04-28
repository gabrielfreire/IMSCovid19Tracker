using IMSCovidTracker.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMSCovidTracker.Services
{
    public class MessageDialogService
    {
        public bool IsDisplayingTutorial = false;
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

        public async Task DisplayTutorial(AbsoluteLayout parentView, Tutorial tutorialComponent, int displayTime = 4000)
        {
            if (IsDisplayingTutorial) return;
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                IsDisplayingTutorial = true;
                parentView.Children.Add(tutorialComponent);
                await Task.Delay(500);
                await tutorialComponent.FadeTo(0.8, 300);
                await Task.Delay(displayTime);
                await tutorialComponent.FadeTo(0, 300);
                parentView.Children.Remove(tutorialComponent);
                IsDisplayingTutorial = false;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMSCovidTracker.Services
{
    public class NavigationService
    {
        public async Task Navigate(Page target, bool isModal = false, bool animated = true)
        {
            if (Device.RuntimePlatform == Device.UWP || isModal)
            { 
                await Shell.Current.Navigation.PushModalAsync(target, animated);
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(target, animated);
            }
        }

        public async Task Navigate(Page source, Page target, bool isModal = false, bool animated = true)
        {
            if (Device.RuntimePlatform == Device.UWP || isModal)
            {
                await source.Navigation.PushModalAsync(target, animated);
            }
            else
            {
                await source.Navigation.PushAsync(target, animated);
            }
        }


        public async Task NavigateBack(bool isModal)
        {
            if (isModal)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                await Shell.Current.Navigation.PopAsync();
            }

            return;
        }

        public async Task NavigateBack(Page page, bool isModal)
        {
            if (isModal)
            {
                await page.Navigation.PopModalAsync();
            }
            else
            {
                await page.Navigation.PopAsync();
            }

            return;
        }
    }
}

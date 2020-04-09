using IMSCovidTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel _viewModel;

        public HomePage()
        {
            BindingContext = _viewModel = new HomeViewModel(this);
            InitializeComponent();
            _ = Task.Run(async () =>
            {
                await _viewModel.LoadCovidData();
                await _viewModel.LoadDefaultWidgets();
            });
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                await _viewModel.LoadCovidData();
                await _viewModel.LoadDefaultWidgets();
            });
        }

        
    }
}
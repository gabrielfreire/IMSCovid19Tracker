using IMSCovidTracker.Models;
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
    public partial class ViewWidgetPage : ContentPage
    {
        private ViewWidgetViewModel _viewModel;

        public ViewWidgetPage(CovidLocation ountryWidget)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewWidgetViewModel(this, ountryWidget);
        }
    }
}
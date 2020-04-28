using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Navbar : ContentView
    {
        public static readonly BindableProperty ShowHelpProperty = BindableProperty.Create(nameof(ShowHelp), typeof(bool), typeof(bool), true);
        public bool ShowHelp { get => (bool)GetValue(ShowHelpProperty); set => SetValue(ShowHelpProperty, value); }

        public ICommand ShowTutorialCommand => new Command(async () => await ShowTutorial());

        public AbsoluteLayout AbsoluteLayoutElement { get; set; }

        public Navbar()
        {
            BindingContext = this;
            InitializeComponent();
        }


        public async Task ShowTutorial()
        {
            if (AbsoluteLayoutElement != null)
            {
                await App.MessageDialogService.DisplayTutorial(AbsoluteLayoutElement, new Tutorial(new Rectangle
                {
                    Left= 0.9,
                    Top= Device.RuntimePlatform == Device.Android ? 0.1 : 0.2,
                    Width=0.6,
                    Height = 0.4
                }));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(ShowHelpProperty))
            {
                helpButton.IsVisible = ShowHelp;
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}
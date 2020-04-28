using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CovidInformationCard : ContentView
    {
        public static readonly BindableProperty CountryNameProperty = BindableProperty.Create(nameof(CountryName), typeof(string), typeof(string), default(string));
        public string CountryName { get => (string)GetValue(CountryNameProperty); set => SetValue(CountryNameProperty, value); }

        public static readonly BindableProperty CountryFlagProperty = BindableProperty.Create(nameof(CountryFlag), typeof(string), typeof(string), default(string));
        public string CountryFlag { get => (string)GetValue(CountryFlagProperty); set => SetValue(CountryFlagProperty, value); }


        public static readonly BindableProperty ConfirmedProperty = BindableProperty.Create(nameof(Confirmed), typeof(int), typeof(int), default(int));
        public int Confirmed { get => (int)GetValue(ConfirmedProperty); set => SetValue(ConfirmedProperty, value); }


        public static readonly BindableProperty DeathsProperty = BindableProperty.Create(nameof(Deaths), typeof(int), typeof(int), default(int));
        public int Deaths { get => (int)GetValue(DeathsProperty); set => SetValue(DeathsProperty, value); }

        public static readonly BindableProperty RecoveredProperty = BindableProperty.Create(nameof(Recovered), typeof(int), typeof(int), default(int));
        public int Recovered { get => (int)GetValue(RecoveredProperty); set => SetValue(RecoveredProperty, value); }

        public static readonly BindableProperty SmallProperty = BindableProperty.Create(nameof(Small), typeof(bool), typeof(bool), false);
        public bool Small { get => (bool)GetValue(SmallProperty); set => SetValue(SmallProperty, value); }

        public CovidInformationCard()
        {
            InitializeComponent();
        }
    }
}
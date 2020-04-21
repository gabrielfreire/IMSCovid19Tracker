using IMSCovidTracker.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LargeCovidInformationCard : ContentView, INotifyPropertyChanged
    {
        private string _countryName;
        private CovidLocation _country;
        private string _formattedCountryHeader;

        public static readonly BindableProperty CountryNameProperty = BindableProperty.Create(nameof(CountryName), typeof(string), typeof(string), default(string));
        public string CountryName
        {
            get
            {
                _countryName = (string)GetValue(CountryNameProperty);
                return _countryName;
            }
            set
            {
                RaiseIfPropertyChanged(ref _countryName, value);
                SetValue(CountryNameProperty, value);
            }
        }

        public static readonly BindableProperty CountryProperty = BindableProperty.Create(nameof(Country), typeof(CovidLocation), typeof(CovidLocation), default(CovidLocation), BindingMode.TwoWay);
        public CovidLocation Country
        {
            get 
            {
                _country = (CovidLocation)GetValue(CountryProperty);
                return _country;
            }
            set 
            {
                RaiseIfPropertyChanged(ref _country, value);
                SetValue(CountryProperty, value);
            }
        }

        public static readonly BindableProperty SmallProperty = BindableProperty.Create(nameof(Small), typeof(bool), typeof(bool), true);
        public bool Small { get => (bool)GetValue(SmallProperty); set => SetValue(SmallProperty, value); }


        public string FormattedCountryHeader { get => _formattedCountryHeader; set => RaiseIfPropertyChanged(ref _formattedCountryHeader, value); }

        public LargeCovidInformationCard()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Country")
            {
                if (Country != null && Country.RecoveredPerDeaths > 0)
                {
                    var entries = new[]
                    {
                        new Microcharts.Entry(Country.Recovered)
                        {
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.LightGreen.ToHex())
                        },
                        new Microcharts.Entry(Country.Deaths)
                        {
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.PaleVioletRed.ToHex())
                        }
                    };
                    var chart = new DonutChart() { Entries = entries, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())};
                    donutView.Chart = chart;
                    donutViewBigger.Chart = chart;
                }

                if (Country != null)
                {
                    var entries = new[]
                    {
                        new Microcharts.Entry(Country.Confirmed)
                        {
                            Label = "Confirmed",
                            ValueLabel = $"{Country.Confirmed}",
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.Orange.ToHex())
                        },
                        new Microcharts.Entry(Country.Active)
                        {
                            Label = "Active",
                            ValueLabel = $"{Country.Active}",
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.Yellow.ToHex())
                        },
                        new Microcharts.Entry(Country.Deaths)
                        {
                            Label = "Deaths",
                            ValueLabel = $"{Country.Deaths}",
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.PaleVioletRed.ToHex())
                        },
                        new Microcharts.Entry(Country.Recovered)
                        {
                            Label = "Recovered",
                            ValueLabel = $"{Country.Recovered}",
                            TextColor = SKColor.Parse(Color.White.ToHex()),
                            Color = SKColor.Parse(Color.LightGreen.ToHex())
                        }
                    };
                    var chart = new BarChart() { Entries = entries, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
                    barViewBigger.Chart = chart;
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaiseIfPropertyChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return;

            property = value;
            OnPropertyChanged(propertyName);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IMSCovidTracker.Converters
{
    public class IsZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value.GetType() == typeof(int)) return (int)value == 0;
            else if (value.GetType() == typeof(double)) return (double)value == 0.0;
            else if (value.GetType() == typeof(Int64)) return (Int64)value == 0.0;
            return (long)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }
    }
}

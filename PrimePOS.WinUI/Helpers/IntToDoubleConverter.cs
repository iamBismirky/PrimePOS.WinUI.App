using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers
{
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is int i ? (double)i : 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double d)
                return (int)Math.Round(d);

            return 0;
        }
    }
}

using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace PrimePOS.WinUI.Helpers
{
    public class NumeroConverterHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "";

            if (decimal.TryParse(value.ToString(), out decimal numero))
            {
                return numero.ToString("N2", CultureInfo.CurrentCulture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}

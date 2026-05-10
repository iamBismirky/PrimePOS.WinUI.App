using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers
{
    public class NumeroConverterHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "RD$ 0.00";

            if (decimal.TryParse(value.ToString(), out decimal numero))
            {
                return $"RD$ {numero:N2}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}

using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers
{


    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            if (value is decimal d)
                return d.ToString("N2");

            return "0.00";
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            if (decimal.TryParse(
                value?.ToString(),
                out decimal result))
            {
                return result;
            }

            return 0m;
        }
    }
}

using System;

namespace PrimePOS.WinUI.Helpers
{


    using Microsoft.UI.Xaml.Data;
    using System.Globalization;

    public class DecimalToStringConverter : IValueConverter
    {
        private static readonly CultureInfo rdCulture =
            new CultureInfo("es-DO");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal d)
            {
                return d.ToString("C2", rdCulture);
            }

            return "RD$0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value?.ToString(),
                NumberStyles.Any,
                rdCulture,
                out decimal result))
            {
                return result;
            }

            return 0m;
        }
    }
}

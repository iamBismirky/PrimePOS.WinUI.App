using Microsoft.UI.Xaml.Data;
using System;


namespace PrimePOS.WinUI.Helpers
{

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int number)
                return number.ToString();

            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (int.TryParse(value?.ToString(), out int result))
                return result;

            return 0;
        }
    }
}

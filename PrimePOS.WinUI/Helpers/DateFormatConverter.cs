
using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers
{
    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
                return date.ToString("dd/MM/yyyy");

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}

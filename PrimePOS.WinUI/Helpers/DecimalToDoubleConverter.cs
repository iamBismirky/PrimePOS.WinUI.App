using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers;

public class DecimalToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (double)(decimal)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (decimal)(double)value;
    }
}
using Microsoft.UI.Xaml.Data;
using System;

namespace PrimePOS.WinUI.Helpers;

public class DecimalToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is decimal d ? (double)d : 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is double d ? (decimal)d : 0m;
    }
}
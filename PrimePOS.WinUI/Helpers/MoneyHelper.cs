using System.Globalization;

namespace PrimePOS.WinUI.Helpers;


public static class MoneyHelper
{
    public static decimal ToDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0m;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result))
            return result;

        return 0m;
    }

    public static string ToString(decimal value)
    {
        return value.ToString("N2", CultureInfo.CurrentCulture);
    }
}
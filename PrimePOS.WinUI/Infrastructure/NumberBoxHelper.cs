using Microsoft.UI.Xaml.Controls;
using Windows.Globalization.NumberFormatting;

namespace PrimePOS.WinUI.Infrastructure
{
    public static class NumberBoxHelper

    {
        public static void AplicarFormatoMoneda(NumberBox numberBox)
        {
            var formatter = new DecimalFormatter
            {
                FractionDigits = 2,
                IntegerDigits = 1,
                IsGrouped = true,
                NumeralSystem = "Latn"
            };

            numberBox.NumberFormatter = formatter;
        }
    }

}

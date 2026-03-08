using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Infrastructure
{
    public static class DialogHelper
    {
        public static async Task MostrarMensaje(XamlRoot xamlRoot, string titulo, string mensaje)
        {
            var dialog = new ContentDialog
            {
                Title = titulo,
                Content = mensaje,
                CloseButtonText = "Aceptar",
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}

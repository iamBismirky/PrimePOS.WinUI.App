using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Helpers
{
    public static class DialogHelper
    {


        private static bool _dialogAbierto = false;

        public static async Task MostrarMensaje(XamlRoot xamlRoot, string titulo, string mensaje)
        {
            if (_dialogAbierto) return; // evita abrir más de uno
            _dialogAbierto = true;

            var dialog = new ContentDialog
            {
                Title = titulo,
                Content = mensaje,
                CloseButtonText = "Ok",
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
            _dialogAbierto = false;
        }

    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.ViewModels;
using System;



namespace PrimePOS.WinUI.Overlays
{

    public sealed partial class CobrarOverlay : UserControl
    {
        public event Action? OnCerrar;
        public CobrarOverlay()
        {
            InitializeComponent();
        }
        private void Cancelar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            OnCerrar?.Invoke();
        }
        private async void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CobrarViewModel vm)
            {
                try
                {
                    await vm.ConfirmarAsync();
                    //OnCerrar?.Invoke();
                }
                catch (Exception ex)
                {
                    await DialogHelper.MostrarMensaje(this.XamlRoot, "Advertencia", ex.ToString());
                }
            }
        }
        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            txtEfectivo.Focus(FocusState.Programmatic);
        }

    }
}

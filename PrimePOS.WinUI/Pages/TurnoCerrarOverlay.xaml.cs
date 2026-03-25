using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Infrastructure;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TurnoCerrarOverlay : UserControl
    {
        public TurnoCerrarOverlay()
        {
            this.InitializeComponent();
        }
        private void BtnCerrarTurno_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            txtTurno.Text = Sesion.TurnoId.ToString();
        }
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PrimePOS.WinUI.Overlays;


public sealed partial class CerrarTurnoOverlay : UserControl
{

    public CerrarTurnoOverlay()
    {

        this.InitializeComponent();
    }

    private void txtEfectivo_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is CerrarTurnoViewModel vm)
        {
            vm.FormatearEfectivo();
        }
    }

}

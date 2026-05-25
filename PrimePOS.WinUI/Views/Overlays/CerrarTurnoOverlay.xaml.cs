using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class CerrarTurnoOverlay : UserControl
{

    public CerrarTurnoOverlay(CerrarTurnoOverlayViewModel vm)
    {

        this.InitializeComponent();
        DataContext = vm;
    }

    private void txtEfectivo_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is CerrarTurnoOverlayViewModel vm)
        {
            vm.FormatearEfectivo();
        }
    }

}

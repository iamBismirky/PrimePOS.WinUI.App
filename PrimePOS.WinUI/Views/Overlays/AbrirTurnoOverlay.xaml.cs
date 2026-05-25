using Microsoft.UI.Xaml.Controls;

namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class AbrirTurnoOverlay : UserControl
{

    public AbrirTurnoOverlay(AbrirTurnoOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

}

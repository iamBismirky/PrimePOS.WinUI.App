using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;

namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class AbrirTurnoOverlay : UserControl
{

    public AbrirTurnoOverlay(AbrirTurnoOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

}

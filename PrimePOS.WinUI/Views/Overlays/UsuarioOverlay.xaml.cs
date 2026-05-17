using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class UsuarioOverlay : UserControl
{
    public UsuarioOverlay(UsuarioOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}

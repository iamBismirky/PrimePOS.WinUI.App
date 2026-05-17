using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays;

public sealed partial class RolOverlay : UserControl
{
    public RolOverlay(RolOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}

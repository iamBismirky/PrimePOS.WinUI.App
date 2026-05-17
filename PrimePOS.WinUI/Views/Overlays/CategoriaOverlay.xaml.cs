using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class CategoriaOverlay : UserControl
{
    public CategoriaOverlay(CategoriaOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}

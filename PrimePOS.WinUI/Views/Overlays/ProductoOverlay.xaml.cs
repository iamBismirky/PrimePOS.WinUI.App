using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;


namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class ProductoOverlay : UserControl
{
    public ProductoOverlay(ProductoOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (DataContext is ProductoOverlayViewModel vm)
        {
            vm.NotificarPrecios();
        }
    }
}

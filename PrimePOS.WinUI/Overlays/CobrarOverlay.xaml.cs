using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Overlays;


public sealed partial class CobrarOverlay : UserControl
{

    public CobrarOverlay()
    {
        InitializeComponent();
    }
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        txtEfectivo.Focus(FocusState.Programmatic);
        txtEfectivo.SelectAll();
    }

    private void txtEfectivo_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is CobrarViewModel vm)
        {
            vm.FormatearEfectivo();
        }
    }


}

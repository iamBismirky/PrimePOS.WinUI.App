using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class ClienteOverlay : UserControl
{

    public ClienteOverlay(ClienteOverlayViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;
    }
}

using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Views.Overlays;


public sealed partial class DialogOverlay : UserControl
{
    public DialogOverlay(DialogOverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}

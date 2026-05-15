using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Overlays;


public sealed partial class DialogOverlay : UserControl
{
    public DialogOverlay(DialogViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}

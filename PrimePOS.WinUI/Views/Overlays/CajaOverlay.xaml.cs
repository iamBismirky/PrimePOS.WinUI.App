using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays
{

    public sealed partial class CajaOverlay : UserControl
    {
        public CajaOverlay(CajaOverlayViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

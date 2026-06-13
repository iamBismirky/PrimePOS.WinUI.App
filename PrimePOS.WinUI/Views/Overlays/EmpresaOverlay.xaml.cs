using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays
{
    public sealed partial class EmpresaOverlay : UserControl
    {
        public EmpresaOverlay(EmpresaOverlayViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}

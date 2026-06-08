using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Overlays;



namespace PrimePOS.WinUI.Views.Overlays
{
    public sealed partial class EmpresaOverlay : UserControl
    {
        private readonly EmpresaOverlayViewModel vm;
        public EmpresaOverlay()
        {
            InitializeComponent();
            vm = App.Services.GetRequiredService<EmpresaOverlayViewModel>();
            DataContext = vm;
        }
    }
}

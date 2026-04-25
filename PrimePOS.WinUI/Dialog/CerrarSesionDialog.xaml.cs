using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CerrarSesionDialog : ContentDialog
    {
        public AppSesionViewModel ViewModel;
        public CerrarSesionDialog()
        {
            this.InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<AppSesionViewModel>();
        }


    }
}

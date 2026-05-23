using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Views.Dialog
{

    public sealed partial class CerrarSesionDialog : ContentDialog
    {
        public AppSesionViewModel ViewModel;
        public CerrarSesionDialog()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<AppSesionViewModel>();
        }


    }
}

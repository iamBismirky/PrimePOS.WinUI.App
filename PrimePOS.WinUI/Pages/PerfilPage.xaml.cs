using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;



namespace PrimePOS.WinUI.Pages;


public sealed partial class PerfilPage : Page
{
    private PerfilViewModel ViewModel;
    public PerfilPage()
    {
        InitializeComponent();
        ViewModel = App.AppServices.GetRequiredService<PerfilViewModel>();
        DataContext = ViewModel;
    }


    private void LimpiarCampos()
    {
        pwdActual.Password = "";
        pwdNueva.Password = "";
        pwdConfirmar.Password = "";
    }
    private void Actual_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.PasswordActual = ((PasswordBox)sender).Password;
    }

    private void Nueva_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.PasswordNueva = ((PasswordBox)sender).Password;
    }

    private void Confirmar_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.Confirmar = ((PasswordBox)sender).Password;
    }
    //private void MostrarError(string msg)
    //{
    //    infoBar.Title = "Error";
    //    infoBar.Message = msg;
    //    infoBar.Severity = InfoBarSeverity.Error;
    //    infoBar.IsOpen = true;

    //    AutoClose();
    //}

    //private void MostrarExito(string msg)
    //{
    //    infoBar.Title = "Éxito";
    //    infoBar.Message = msg;
    //    infoBar.Severity = InfoBarSeverity.Success;
    //    infoBar.IsOpen = true;

    //    AutoClose();
    //}

    //private async void AutoClose()
    //{
    //    await Task.Delay(3000);
    //    infoBar.IsOpen = false;
    //}
    //private void MostrarError(string mensaje)
    //{
    //    infoError.Message = mensaje;
    //    infoError.IsOpen = true;

    //    AutoCloseInfoBar();
    //}
    //private async void AutoCloseInfoBar()
    //{
    //    await Task.Delay(3000);
    //    infoError.IsOpen = false;
    //}
}

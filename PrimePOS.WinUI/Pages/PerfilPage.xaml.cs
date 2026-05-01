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

}

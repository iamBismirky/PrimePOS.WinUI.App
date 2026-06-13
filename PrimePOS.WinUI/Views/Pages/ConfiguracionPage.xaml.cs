using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;



namespace PrimePOS.WinUI.Views.Pages;


public sealed partial class ConfiguracionPage : Page
{
    public ConfiguracionPage()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
public partial class ConfiguracionPage
{
    private void BtnGestionUsuarios_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(UsuarioPage));
    }
    private void BtnGestionRoles_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(RolPage));
    }
    private void BtnGestionProductos_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ProductoPage));
    }
    private void BtnGestionCategorias_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CategoriaPage));
    }
    private void BtnGestionClientes_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ClientePage));
    }
    private void BtnGestionCajas_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CajaPage));
    }
    private void BtnGestionEmpresa_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(EmpresaPage));
    }
    private void Btn_SystemAdministration_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AdministracionPage));
    }
}

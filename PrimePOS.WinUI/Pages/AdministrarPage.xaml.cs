using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;



namespace PrimePOS.WinUI.Pages;


public sealed partial class AdministrarPage : Page
{
    public AdministrarPage()
    {
        InitializeComponent();
    }
}
public partial class AdministrarPage
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
}

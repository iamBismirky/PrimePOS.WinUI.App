using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.WinUI.Infrastructure;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UsuarioPage : Page
{
    public UsuarioPage()
    {
        InitializeComponent();
        
    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ListarRoles();
    }
    public void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
    public async void BtnGuardar_Click(object sender, RoutedEventArgs e)
    {
    //    try
    //    {
    //        var service = new UsuarioApiService();

    //        var nuevoUsuario = new CrearUsuarioDto
    //            {
    //                Nombre = txtNombre.Text,
    //                Apellido = txtApellido.Text,
    //                Username = txtUsername.Text,
    //                Password = pwdPassword.Password,
    //                RolId = (int)cbRol.SelectedValue,
    //                Estado = (bool)toggleEstado.IsOn


    //        };
    //        bool resultado =  await service.CrearUsuarioAsync(nuevoUsuario);

    //         if (resultado)
    //        {
    //            var dialog = new ContentDialog
    //            {
    //                Title = "Éxito",
    //                Content = "Usuario creado exitosamente.",
    //                CloseButtonText = "Aceptar"
    //            };
    //             await dialog.ShowAsync();
    //            Frame.GoBack();
    //        }
    //        else
    //        {
    //            var dialog = new ContentDialog
    //            {
    //                Title = "Error",
    //                Content = "No se pudo crear el usuario.",
    //                CloseButtonText = "Aceptar",
    //                XamlRoot = this.XamlRoot
    //            };
    //             await dialog.ShowAsync();
    //        }

    //    }
    //    catch(Exception ex) {
    //        ContentDialog errorDialog = new ContentDialog
    //        {
    //            Title = "Error",
    //            Content = ex.Message,
    //            CloseButtonText = "OK",
    //            XamlRoot = this.XamlRoot
    //        };
    //        await errorDialog.ShowAsync();
    //    }
    }
    private void BtnCrear_Click(object sender, RoutedEventArgs e) { }
    private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
    private void BtnEliminar_Click(object sender, RoutedEventArgs e) { }
    private void BtnLimpiar_Click(object sender, RoutedEventArgs e) { }
    private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    
    private async Task ListarRoles()
    {
        var roles =  await Servicios.RolService.ListarRolesAsync();
        if(roles != null)
        {
            cmbRol.ItemsSource = roles;
        }
        

    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RolPage : Page
{
    
    public RolPage()
    {
        InitializeComponent();
        _=CargarRoles();
        
    }
    private async void BtnCrearRol_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dto = new CrearRolDto
            {
                Descripcion = txtDescripcion.Text,
                Estado = (bool)toggleEstado.IsOn

            };

            await Servicios.RolService.CrearRolAsync(dto);
            await CargarRoles();
            LimpiarCampos();


            
        }
        catch (Exception ex)
        {

            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = ex.Message,
                CloseButtonText = "Aceptar",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

    }
    private async void BtnActualizarRol_Click(object sender, RoutedEventArgs e) { }
    private async void BtnEliminarRol_Click(object sender, RoutedEventArgs e) { }

    private void LimpiarCampos()
    {
        txtDescripcion.Text = "";
    }
    private async Task CargarRoles()
    {
        var roles = await Servicios.RolService.ListarRolesAsync();
        foreach (var r in roles)
        {
            System.Diagnostics.Debug.WriteLine(r.Descripcion);
        }

        dgRoles.ItemsSource = roles;
    }
}




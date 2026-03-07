using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private int _rolIdSeleccionado = 0;
    
    public RolPage()
    {
        InitializeComponent();
        _=CargarRoles();
        
    }
    private async void BtnCrearRol_Click(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(tgEstado.IsOn);
        try
        {
            var dto = new CrearRolDto
            {
                Descripcion = txtDescripcion.Text,
                Estado = tgEstado.IsOn

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
    private async void BtnActualizarRol_Click(object sender, RoutedEventArgs e) 
    {
        try
        {
            if (_rolIdSeleccionado == 0)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Debe de seleccionar un rol para editar",
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.XamlRoot
                };
                return;

            }

            var dto = new ActualizarRolDto
            {
                RolId = _rolIdSeleccionado,
                Descripcion = txtDescripcion.Text,
                Estado = (bool)tgEstado.IsOn

            };

            await Servicios.RolService.ActualizarRolAsync(dto);
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
    private async void BtnEliminarRol_Click(object sender, RoutedEventArgs e) 
    {
        try
        {
            if (_rolIdSeleccionado == 0)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Debe de seleccionar un rol para eliminar",
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.XamlRoot
                };
                return;

            }

            var dto = new EliminarRolDto
            {
                RolId = _rolIdSeleccionado

            };

            await Servicios.RolService.EliminarRolAsync(dto);
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
    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        LimpiarCampos();
    }

    private void dgRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        if (dgRoles.SelectedItem is ListaRolesDto rol)
        {
            System.Diagnostics.Debug.WriteLine("Fila seleccionada");
            _rolIdSeleccionado = rol.RolId;
            txtDescripcion.Text = rol.Descripcion.ToString();
            tgEstado.IsOn = rol.Estado;
            System.Diagnostics.Debug.WriteLine(_rolIdSeleccionado);
        }
    }
    private void LimpiarCampos()
    {
        txtDescripcion.Text = "";
        _rolIdSeleccionado = 0;
        tgEstado.IsOn = true;
    }
    private async Task CargarRoles()
    {
        List<ListaRolesDto> listaRoles = await Servicios.RolService.ListarRolesAsync();
        
        dgRoles.ItemsSource = listaRoles;
        
    }
}




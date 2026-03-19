using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        this.Loaded += Page_Loaded;

    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await CargarRoles();
    }
    private async void BtnCrearRol_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            var dto = new CrearRolDto
            {
                Nombre = txtNombre.Text,
                Estado = tgEstado.IsOn

            };

            await Servicios.RolService.CrearRolAsync(dto);
            await CargarRoles();
            LimpiarCampos();

        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


        }

    }
    private async void BtnActualizarRol_Click(object sender, RoutedEventArgs e)
    {
        try
        {

            var dto = new ActualizarRolDto
            {
                RolId = _rolIdSeleccionado,
                Nombre = txtNombre.Text,
                Estado = (bool)tgEstado.IsOn

            };

            await Servicios.RolService.ActualizarRolAsync(dto);
            await CargarRoles();
            LimpiarCampos();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private async void BtnEliminarRol_Click(object sender, RoutedEventArgs e)
    {
        try
        {


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
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }

    }
    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        LimpiarCampos();
    }

    private async void dgRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {

            if (dgRoles.SelectedItem is ListaRolesDto rol)
            {

                _rolIdSeleccionado = rol.RolId;
                txtNombre.Text = rol.Nombre.ToString();
                tgEstado.IsOn = rol.Estado;
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private void LimpiarCampos()
    {
        txtNombre.Text = "";
        _rolIdSeleccionado = 0;
        tgEstado.IsOn = true;
    }
    private async Task CargarRoles()
    {
        try
        {
            List<ListaRolesDto> listaRoles = await Servicios.RolService.ListarRolesAsync();
            dgRoles.ItemsSource = null;
            dgRoles.ItemsSource = listaRoles;
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }

    }
}




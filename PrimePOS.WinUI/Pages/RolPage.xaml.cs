using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages;


public sealed partial class RolPage : Page
{
    private readonly RolService _rolService;
    private int _rolIdSeleccionado = 0;

    public RolPage()
    {
        InitializeComponent();

        _rolService = App.Services.GetRequiredService<RolService>();

    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await CargarRoles();
    }
    private async void BtnCrear_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            var dto = new CrearRolDto
            {
                Nombre = txtNombre.Text,
                Estado = tsEstado.IsOn

            };

            await _rolService.CrearRolAsync(dto);
            await CargarRoles();
            LimpiarCampos();

        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


        }

    }
    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        try
        {

            var dto = new ActualizarRolDto
            {
                RolId = _rolIdSeleccionado,
                Nombre = txtNombre.Text,
                Estado = (bool)tsEstado.IsOn

            };

            await _rolService.ActualizarRolAsync(dto);
            await CargarRoles();
            LimpiarCampos();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        try
        {


            var dto = new EliminarRolDto
            {
                RolId = _rolIdSeleccionado

            };

            await _rolService.EliminarRolAsync(dto);
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
                tsEstado.IsOn = rol.Estado;
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
        tsEstado.IsOn = true;
    }
    private async Task CargarRoles()
    {
        try
        {
            List<ListaRolesDto> listaRoles = await _rolService.ListarRolesAsync();
            dgRoles.ItemsSource = null;
            dgRoles.ItemsSource = listaRoles;
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }

    }
}




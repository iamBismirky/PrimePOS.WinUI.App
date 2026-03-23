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
        this.Loaded += Page_Loaded;

        _rolService = App.Services.GetRequiredService<RolService>();

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

            await _rolService.CrearRolAsync(dto);
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

            await _rolService.ActualizarRolAsync(dto);
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




using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages;


public sealed partial class RolPage : Page
{

    private RolDto? _rolSeleccionado;
    private readonly RolApiService _rolApiService;
    private List<RolDto> _listaRoles = new();
    private bool _isLoading;
    public RolPage()
    {
        InitializeComponent();


        _rolApiService = App.AppServices.GetRequiredService<RolApiService>();

    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await CargarRolesAsync();
    }


    private void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtBuscar.Focus(FocusState.Programmatic);
        OverlayRol.Visibility = Visibility.Collapsed;
        _rolSeleccionado = null;
    }
    private async Task CargarRolesAsync()
    {
        try
        {
            IsLoading = true;

            _listaRoles = await _rolApiService.GetRolesAsync();
            dgRoles.ItemsSource = null;
            dgRoles.ItemsSource = _listaRoles;
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
        finally { IsLoading = false; }

    }
    private void FiltrarRoles()
    {
        var texto = txtBuscar.Text?.ToLower() ?? "";

        var filtrados = string.IsNullOrWhiteSpace(texto)
            ? _listaRoles
            : _listaRoles
                .Where(r => r.Nombre.ToLower().Contains(texto))
                .ToList();

        dgRoles.ItemsSource = null;
        dgRoles.ItemsSource = filtrados;
    }
    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        FiltrarRoles();
    }
    private void Buscar_Click(object sender, RoutedEventArgs e)
    {
        FiltrarRoles();
    }
    private void AbrirOverlay_Click(object sender, RoutedEventArgs e)
    {
        txtTitulo.Text = "Crear Rol";
        txtNombre.Focus(FocusState.Programmatic);
        OverlayRol.Visibility = Visibility.Visible;
    }
    private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
    {
        OverlayRol.Visibility = Visibility.Collapsed;
        LimpiarCampos();
    }
    private async void Editar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            txtTitulo.Text = "Actualizar Rol";
            txtNombre.Focus(FocusState.Programmatic);

            var btn = sender as Button;
            var rol = btn?.Tag as RolDto;

            if (rol == null) return;

            _rolSeleccionado = rol;


            txtNombre.Text = rol.Nombre;


            OverlayRol.Visibility = Visibility.Visible;
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Advertencia", ex.ToString());
            throw;
        }

    }
    private async void Eliminar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is Button btn && btn.Tag is RolDto rol)
            {

                var dialog = new ContentDialog
                {
                    Title = "Confirmar",
                    Content = $"¿Desactivar el rol '{rol.Nombre}'?",
                    PrimaryButtonText = "Sí",
                    CloseButtonText = "Cancelar",
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result != ContentDialogResult.Primary)
                    return;


                await _rolApiService.DesactivarRolAsync(rol.RolId);


                await CargarRolesAsync();
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }


    private async void GuardarOverlay_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!BtnGuardar.IsEnabled) return;

            SetLoadingButton(true);
            if (_rolSeleccionado == null)
            {
                var dto = new CrearRolDto
                {
                    Nombre = txtNombre.Text.Trim(),

                };

                await _rolApiService.CrearRolAsync(dto);
            }
            else
            {
                var rolDto = new ActualizarRolDto
                {
                    RolId = _rolSeleccionado.RolId,
                    Nombre = txtNombre.Text.Trim()
                };
                await _rolApiService.ActualizarRolAsync(rolDto.RolId, rolDto);

            }

            await CargarRolesAsync();
            LimpiarCampos();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


        }
        finally { SetLoadingButton(false); }

    }
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OverlayLoading.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    private void SetLoadingButton(bool isLoading)
    {
        prCrear.IsActive = isLoading;
        prCrear.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

        BtnGuardar.IsEnabled = !isLoading;
        txtGuardar.Text = isLoading ? "Guardando..." : "Guardar";
    }
}




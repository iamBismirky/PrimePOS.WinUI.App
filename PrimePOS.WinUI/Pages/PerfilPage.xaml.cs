using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;



namespace PrimePOS.WinUI.Pages;


public sealed partial class PerfilPage : Page
{
    private readonly UsuarioService _usuarioService;
    private readonly SesionService _sesionService;
    public PerfilPage()
    {
        InitializeComponent();
        CargarDatos();

        _usuarioService = App.Services.GetRequiredService<UsuarioService>();
        _sesionService = App.Services.GetRequiredService<SesionService>();
    }
    private async void BtnActualizarPassword_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dto = new CambiarContraseñaDto
            {
                UsuarioId = _sesionService.UsuarioActual!.UsuarioId,
                ContraseñaActual = pwdActual.Password.Trim(),
                ContraseñaNueva = pwdNueva.Password.Trim(),
                Confirmar = pwdConfirmar.Password.Trim(),


            };
            await _usuarioService.CambiarContraseñaAsync(dto);
            LimpiarCampos();
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Exito", "Contraseña actualizada correctamente");
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private void CargarDatos()
    {
        txtUsuario.Text = Sesion.UsuarioNombre;
        txtRol.Text = Sesion.RolNombre;
    }
    private void LimpiarCampos()
    {
        pwdActual.Password = "";
        pwdNueva.Password = "";
        pwdConfirmar.Password = "";
    }
}

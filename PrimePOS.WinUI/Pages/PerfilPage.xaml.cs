using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.ViewModels;
using System;



namespace PrimePOS.WinUI.Pages;


public sealed partial class PerfilPage : Page
{
    private readonly UsuarioService _usuarioService;
    private readonly AppSesionViewModel _sesion;


    public PerfilPage()
    {
        InitializeComponent();
        CargarDatos();

        _usuarioService = App.Services.GetRequiredService<UsuarioService>();
        _sesion = App.Services.GetRequiredService<AppSesionViewModel>();
        this.DataContext = _sesion;
    }
    private async void BtnActualizarPassword_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dto = new CambiarContraseñaDto
            {
                UsuarioId = _sesion.UsuarioActual!.UsuarioId,
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
        //txtUsuario.Text = Sesion.UsuarioNombre;
        //txtRol.Text = Sesion.RolNombre;
    }
    private void LimpiarCampos()
    {
        pwdActual.Password = "";
        pwdNueva.Password = "";
        pwdConfirmar.Password = "";
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PerfilPage : Page
{
    public PerfilPage()
    {
        InitializeComponent();
        CargarDatos();
    }
    private async void BtnActualizarPassword_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dto = new CambiarContraseñaDto
            {
                UsuarioId = Sesion.UsuarioId,
                ContraseñaActual = pwdActual.Password.Trim(),
                ContraseñaNueva = pwdNueva.Password.Trim(),
                Confirmar = pwdConfirmar.Password.Trim(),


            };
            await Servicios.UsuarioService.CambiarContraseñaAsync(dto);
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

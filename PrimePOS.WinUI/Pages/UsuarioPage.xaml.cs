using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages;


public sealed partial class UsuarioPage : Page
{
    private readonly UsuarioService _usuarioService;
    private readonly RolService _rolService;
    private int usuarioIdSeleccionado = 0;
    public UsuarioPage()
    {
        InitializeComponent();

        _usuarioService = App.Services.GetRequiredService<UsuarioService>();
        _rolService = App.Services.GetRequiredService<RolService>();

    }
    public void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        try
        {
            //Cargar listado de usuarios (DataGrid) y Roles (ComboBox)
            await ListarRoles();
            await ListarUsuariosAsync();

            //Foco en el textBox Nombre
            txtNombre.Focus(FocusState.Programmatic);

        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }


    public async void BtnCrear_Click(object sender, RoutedEventArgs e)
    {
        try
        {


            var usuario = new CrearUsuarioDto
            {
                Nombre = txtNombre.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = pwdPassword.Password.Trim(),
                RolId = cmbRol.SelectedValue != null ? (int)cmbRol.SelectedValue : 0,
                Estado = (bool)tsEstado.IsOn


            };
            await _usuarioService.CrearUsuarioAsync(usuario);

            await ListarUsuariosAsync();
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


            var usuario = new ActualizarUsuarioDto
            {
                UsuarioId = usuarioIdSeleccionado,
                Nombre = txtNombre.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                RolId = cmbRol.SelectedValue != null ? (int)cmbRol.SelectedValue : 0,
                Estado = tsEstado.IsOn,

            };
            await _usuarioService.ActualizarUsuarioAsync(usuario);

            await ListarUsuariosAsync();
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
            var usuario = new EliminarUsuarioDto
            {
                UsuarioId = usuarioIdSeleccionado,


            };
            await _usuarioService.EliminarUsuarioAsync(usuario);

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Exito", "Usuario eliminado correctamente");

            await ListarUsuariosAsync();
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
    private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgUsuarios.SelectedItem is UsuarioDto dto)
        {
            usuarioIdSeleccionado = dto.UsuarioId;
            txtNombre.Text = dto.Nombre;
            txtApellidos.Text = dto.Apellidos;
            txtUsername.Text = dto.Username;
            cmbRol.SelectedValue = dto.RolId;
            tsEstado.IsOn = dto.Estado;

        }
        pwdPassword.IsEnabled = false;
    }

    private async Task ListarRoles()
    {
        try
        {
            var roles = await _rolService.ListarRolesAsync();
            if (roles != null)
            {

                cmbRol.ItemsSource = roles;
                cmbRol.DisplayMemberPath = "Nombre";
                cmbRol.SelectedValuePath = "RolId";
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


        }
    }
    private async Task ListarUsuariosAsync()
    {
        try
        {
            var lista = await _usuarioService.ListarUsuariosAsync();
            dgUsuarios.ItemsSource = lista;
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtApellidos.Text = "";
        txtUsername.Text = "";
        pwdPassword.Password = "";
        cmbRol.SelectedIndex = -1;
        tsEstado.IsOn = true;
        usuarioIdSeleccionado = 0;
        pwdPassword.IsEnabled = true;
    }


}

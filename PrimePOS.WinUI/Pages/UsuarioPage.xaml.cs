using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class UsuarioPage : Page
    {
        private UsuarioDto? _usuarioSeleccionado;
        private readonly UsuarioApiService _usuarioApiService;
        private readonly RolApiService _rolApiService;
        private List<UsuarioDto> _listaUsuarios = new();
        private List<RolDto> _listaRoles = new();

        private bool _isLoading;
        public UsuarioPage()
        {
            InitializeComponent();

            _usuarioApiService = App.AppServices.GetRequiredService<UsuarioApiService>();
            _rolApiService = App.AppServices.GetRequiredService<RolApiService>();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarUsuariosAsync();
        }


        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtUsername.Text = "";
            pwdPassword.Password = "";
            txtFecha.Text = "";
            txtCodigo.Text = "";
            txtBuscar.Focus(FocusState.Programmatic);
            OverlayUsuario.Visibility = Visibility.Collapsed;
            _usuarioSeleccionado = null;
        }
        private async Task CargarUsuariosAsync()
        {
            try
            {
                IsLoading = true;

                _listaUsuarios = await _usuarioApiService.ObtenerUsuariosAsync();
                dgUsuarios.ItemsSource = null;
                dgUsuarios.ItemsSource = _listaUsuarios;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
            finally { IsLoading = false; }

        }
        private async Task CargarRolesAsync()
        {
            try
            {
                IsLoading = true;

                //_listaRoles = await _rolApiService.ObtenerRolesAsync();
                cmbRol.ItemsSource = null;
                cmbRol.ItemsSource = _listaRoles;
                cmbRol.DisplayMemberPath = "Nombre";
                cmbRol.SelectedValuePath = "RolId";
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
            finally { IsLoading = false; }

        }
        private void FiltrarProductos()
        {
            var texto = txtBuscar.Text?.ToLower() ?? "";

            var filtrados = string.IsNullOrWhiteSpace(texto)
                ? _listaUsuarios
                : _listaUsuarios
                    .Where(r => r.Nombre.ToLower().Contains(texto))
                    .ToList();

            dgUsuarios.ItemsSource = null;
            dgUsuarios.ItemsSource = filtrados;
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProductos();
        }
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            FiltrarProductos();
        }
        private async void AbrirOverlay_Click(object sender, RoutedEventArgs e)
        {
            txtTitulo.Text = "Crear Usuario";
            txtCodigo.Text = "------";
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtNombre.Focus(FocusState.Programmatic);
            OverlayUsuario.Visibility = Visibility.Visible;
            await CargarRolesAsync();
        }
        private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
        {
            OverlayUsuario.Visibility = Visibility.Collapsed;
            LimpiarCampos();
        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitulo.Text = "Visualizar Usuario";
                txtNombre.Focus(FocusState.Programmatic);
                await CargarRolesAsync();
                var btn = sender as Button;
                var usuario = btn?.Tag as UsuarioDto;
                if (usuario == null) return;

                _usuarioSeleccionado = usuario;
                txtCodigo.Text = usuario.Codigo.ToString();
                txtFecha.Text = usuario.FechaRegistro.ToString("dd/MM/yyyy");
                txtNombre.Text = usuario.Nombre;
                txtApellidos.Text = usuario.Apellidos;
                txtUsername.Text = usuario.Username;
                cmbRol.SelectedValue = usuario.RolId;
                tsEstado.IsOn = usuario.Estado;


                OverlayUsuario.Visibility = Visibility.Visible;
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
                if (sender is Button btn && btn.Tag is UsuarioDto usuario)
                {

                    var dialog = new ContentDialog
                    {
                        Title = "Confirmar",
                        Content = $"¿Desactivar el usuario '{usuario.Nombre}'?",
                        CloseButtonText = "Cancelar",
                        PrimaryButtonText = "Sí",
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result != ContentDialogResult.Primary)
                        return;



                    await _usuarioApiService.DesactivarUsuarioAsync(usuario.UsuarioId);


                    await CargarUsuariosAsync();
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
                if (_usuarioSeleccionado == null)
                {
                    var dto = new CrearUsuarioDto
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        Username = txtUsername.Text.Trim(),
                        Password = pwdPassword.Password.Trim(),
                        RolId = (int)(cmbRol.SelectedValue ?? 0),
                        Estado = tsEstado.IsOn,

                    };

                    await _usuarioApiService.CrearUsuarioAsync(dto);
                    await CargarUsuariosAsync();
                    LimpiarCampos();
                }
                else
                {
                    var dto = new ActualizarUsuarioDto
                    {
                        UsuarioId = _usuarioSeleccionado.UsuarioId,
                        Nombre = txtNombre.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        RolId = (int)(cmbRol.SelectedValue ?? 0),
                        Estado = tsEstado.IsOn,
                    };
                    await _usuarioApiService.ActualizarUsuarioAsync(dto.UsuarioId, dto);
                    await CargarUsuariosAsync();
                    LimpiarCampos();

                }


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
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class ClientePage : Page
    {
        private ClienteDto? _clienteSeleccionado;
        private readonly ClienteApiService _clienteApiService;
        private List<ClienteDto> _listaClientes = new();
        private bool _isLoading;
        public ClientePage()
        {
            InitializeComponent();

            _clienteApiService = App.AppServices.GetRequiredService<ClienteApiService>();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarClientesAsync();
        }


        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDocumento.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            txtFecha.Text = "";
            txtCodigo.Text = "";
            txtBuscar.Focus(FocusState.Programmatic);
            OverlayCliente.Visibility = Visibility.Collapsed;
            _clienteSeleccionado = null;
        }
        private async Task CargarClientesAsync()
        {
            try
            {
                IsLoading = true;

                _listaClientes = await _clienteApiService.ObtenerClientesAsync();
                dgClientes.ItemsSource = null;
                dgClientes.ItemsSource = _listaClientes;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
            finally { IsLoading = false; }

        }
        private void FiltrarClientes()
        {
            var texto = txtBuscar.Text?.ToLower() ?? "";

            var filtrados = string.IsNullOrWhiteSpace(texto)
                ? _listaClientes
                : _listaClientes
                    .Where(r => r.Nombre.ToLower().Contains(texto))
                    .ToList();

            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = filtrados;
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarClientes();
        }
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            FiltrarClientes();
        }
        private void AbrirOverlay_Click(object sender, RoutedEventArgs e)
        {
            txtTitulo.Text = "Crear Cliente";
            txtCodigo.Text = "------";
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtNombre.Focus(FocusState.Programmatic);
            OverlayCliente.Visibility = Visibility.Visible;
        }
        private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
        {
            OverlayCliente.Visibility = Visibility.Collapsed;
            LimpiarCampos();
        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitulo.Text = "Actualizar Cliente";
                txtNombre.Focus(FocusState.Programmatic);

                var btn = sender as Button;
                var cliente = btn?.Tag as ClienteDto;
                if (cliente == null) return;

                _clienteSeleccionado = cliente;
                txtCodigo.Text = cliente.Codigo.ToString();
                txtFecha.Text = cliente.FechaRegistro.ToString("dd/MM/yyyy");
                txtNombre.Text = cliente.Nombre;
                txtDocumento.Text = cliente.Documento;
                txtEmail.Text = cliente.Email;
                txtTelefono.Text = cliente.Telefono;
                txtDireccion.Text = cliente.Direccion;
                tsEstado.IsOn = cliente.Estado;


                OverlayCliente.Visibility = Visibility.Visible;
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
                if (sender is Button btn && btn.Tag is ClienteDto cliente)
                {

                    var dialog = new ContentDialog
                    {
                        Title = "Confirmar",
                        Content = $"¿Desactivar el cliente '{cliente.Nombre}'?",
                        CloseButtonText = "Cancelar",
                        PrimaryButtonText = "Sí",
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result != ContentDialogResult.Primary)
                        return;



                    await _clienteApiService.DesactivarClienteAsync(cliente.ClienteId);


                    await CargarClientesAsync();
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
                if (_clienteSeleccionado == null)
                {
                    var dto = new CrearClienteDto
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Documento = txtDocumento.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Direccion = txtDireccion.Text.Trim(),
                        Estado = tsEstado.IsOn,

                    };

                    await _clienteApiService.CrearClienteAsync(dto);
                    await CargarClientesAsync();
                    LimpiarCampos();
                }
                else
                {
                    var dto = new ActualizarClienteDto
                    {
                        ClienteId = _clienteSeleccionado.ClienteId,
                        Nombre = txtNombre.Text.Trim(),
                        Documento = txtDocumento.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Direccion = txtDireccion.Text.Trim(),
                        Estado = tsEstado.IsOn,
                    };
                    await _clienteApiService.ActualizarClienteAsync(dto.ClienteId, dto);
                    await CargarClientesAsync();
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

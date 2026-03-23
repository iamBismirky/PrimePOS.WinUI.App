using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages
{

    public sealed partial class ClientePage : Page
    {
        private readonly ClienteService _clienteService;
        private int _clienteIdSeleccionado = 0;
        public ClientePage()
        {
            InitializeComponent();

            _clienteService = App.Services.GetRequiredService<ClienteService>();

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ListarClientesAsync();
        }
        private async void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dto = new CrearClienteDto
                {
                    Nombre = txtNombre.Text,
                    Documento = txtDocumento.Text,
                    Email = txtEmail.Text,
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    Estado = tsEstado.IsOn

                };
                await _clienteService.CrearClienteAsync(dto);
                await ListarClientesAsync();
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
                var dto = new ActualizarClienteDto
                {
                    ClienteId = _clienteIdSeleccionado,
                    Nombre = txtNombre.Text,
                    Documento = txtDocumento.Text,
                    Email = txtEmail.Text,
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    Estado = tsEstado.IsOn

                };
                await _clienteService.ActualizarClienteAsync(dto);
                await ListarClientesAsync();
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
                var dto = new EliminarClienteDto
                {
                    ClienteId = _clienteIdSeleccionado

                };
                await _clienteService.EliminarClienteAsync(dto);
                await ListarClientesAsync();
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

        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClientes.SelectedItem is ClienteDto dto)
            {
                _clienteIdSeleccionado = dto.ClienteId;
                txtNombre.Text = dto.Nombre;
                txtDireccion.Text = dto.Direccion;
                txtDocumento.Text = dto.Documento;
                txtEmail.Text = dto.Email;
                txtTelefono.Text = dto.Telefono;
                tsEstado.IsOn = dto.Estado;

            }

        }
        private async Task ListarClientesAsync()
        {
            try
            {
                var lista = await _clienteService.ListarClientes();
                dgClientes.ItemsSource = lista;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDocumento.Text = "";
            txtDireccion.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            tsEstado.IsOn = true;
            _clienteIdSeleccionado = 0;


        }
    }
}

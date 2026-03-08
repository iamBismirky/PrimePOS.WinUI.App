using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.WinUI.Infrastructure;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientePage : Page
    {
        private int _clienteIdSeleccionado = 0;
        public ClientePage()
        {
            InitializeComponent();
            ListarClientes();
        }
        private async void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            var dto = new ClienteDto
            {
                Nombre = txtNombre.Text,
                Documento = txtDocumento.Text,
                Email = txtEmail.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDireccion.Text,
                Estado = tsEstado.IsOn

            };
            await Servicios.ClienteService.CrearClienteAsync(dto);
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClientes.SelectedItem is ClienteDto dto)
            {
                dto.ClienteId = _clienteIdSeleccionado;
                dto.Nombre = txtNombre.Text;
                dto.Documento = txtDocumento.Text;
                dto.Direccion = txtDireccion.Text;
                dto.Telefono = txtTelefono.Text;
                dto.Email = txtEmail.Text;
                dto.Estado = tsEstado.IsOn;
            }

        }
        private async Task ListarClientes()
        {
            var clientes = await Servicios.ClienteService.ListarClientes();
            dgClientes.ItemsSource = clientes;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";

        }
    }
}

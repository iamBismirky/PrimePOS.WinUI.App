using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GestionCajasPage : Page
    {
        private int _cajaIdSeleccionado = 0;
        public GestionCajasPage()
        {
            InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ListarCajasAsync();
        }
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var dto = new CajaDto
                {
                    CajaId = _cajaIdSeleccionado

                };

                await Servicios.CajaService.EliminarCajaAsync(dto);
                await ListarCajasAsync();
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

                var dto = new CajaDto
                {
                    CajaId = _cajaIdSeleccionado,
                    Nombre = txtNombre.Text,
                    Estado = (bool)tsEstado.IsOn

                };

                await Servicios.CajaService.ActualizarCajaAsync(dto);
                await ListarCajasAsync();
                LimpiarCampos();


            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
        }

        private async void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dto = new CajaDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Estado = tsEstado.IsOn
                };

                await Servicios.CajaService.CrearCajaAsync(dto);
                await ListarCajasAsync();
                LimpiarCampos();
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


            }
        }

        private void dgCajas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCajas.SelectedItem is CajaDto caja)
            {
                _cajaIdSeleccionado = caja.CajaId;
                txtNombre.Text = caja.Nombre;
                tsEstado.IsOn = caja.Estado;
            }

        }

        private async Task ListarCajasAsync()
        {
            var lista = await Servicios.CajaService.ListarCajasAsync();

            dgCajas.ItemsSource = lista;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            tsEstado.IsOn = true;
            _cajaIdSeleccionado = 0;
        }
    }
}

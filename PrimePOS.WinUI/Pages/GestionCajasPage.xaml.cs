using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages
{

    public sealed partial class GestionCajasPage : Page
    {
        private readonly CajaService _cajaService;
        private int _cajaIdSeleccionado = 0;
        public GestionCajasPage()
        {
            InitializeComponent();

            _cajaService = App.Services.GetRequiredService<CajaService>();
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

                await _cajaService.EliminarCajaAsync(dto);
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

                await _cajaService.ActualizarCajaAsync(dto);
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

                await _cajaService.CrearCajaAsync(dto);
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
            var lista = await _cajaService.ListarCajasAsync();

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

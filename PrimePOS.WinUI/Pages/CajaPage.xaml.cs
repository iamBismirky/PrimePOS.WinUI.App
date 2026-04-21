using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CajaPage : Page
    {
        private CajaDto? _cajaSeleccionada;
        private readonly CajaApiService _cajaApiService;
        private List<CajaDto> _listaCajas = new();
        private bool _isLoading;
        public CajaPage()
        {
            InitializeComponent();

            _cajaApiService = App.AppServices.GetRequiredService<CajaApiService>();

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarCajasAsync();
        }


        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtBuscar.Focus(FocusState.Programmatic);
            OverlayCaja.Visibility = Visibility.Collapsed;
            _cajaSeleccionada = null;
        }
        private async Task CargarCajasAsync()
        {
            //try
            //{
            //    IsLoading = true;

            //    _listaCajas = await _cajaApiService.ObtenerCajasAsync();
            //    dgCajas.ItemsSource = null;
            //    dgCajas.ItemsSource = _listaCajas;
            //}
            //catch (Exception ex)
            //{

            //    await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            //}
            //finally { IsLoading = false; }

        }
        private void FiltrarCategorias()
        {
            var texto = txtBuscar.Text?.ToLower() ?? "";

            var filtrados = string.IsNullOrWhiteSpace(texto)
                ? _listaCajas
                : _listaCajas
                    .Where(r => r.Nombre.ToLower().Contains(texto))
                    .ToList();

            dgCajas.ItemsSource = null;
            dgCajas.ItemsSource = filtrados;
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarCategorias();
        }
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            FiltrarCategorias();
        }
        private void AbrirOverlay_Click(object sender, RoutedEventArgs e)
        {
            txtTitulo.Text = "Crear Caja";
            txtNombre.Focus(FocusState.Programmatic);
            OverlayCaja.Visibility = Visibility.Visible;
        }
        private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
        {
            OverlayCaja.Visibility = Visibility.Collapsed;
            LimpiarCampos();
        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitulo.Text = "Actualizar Caja";
                txtNombre.Focus(FocusState.Programmatic);

                var btn = sender as Button;
                var caja = btn?.Tag as CajaDto;

                if (caja == null) return;

                _cajaSeleccionada = caja;


                txtNombre.Text = caja.Nombre;


                OverlayCaja.Visibility = Visibility.Visible;
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

                var button = sender as Button;
                var caja = button?.Tag as CajaDto;

                if (caja == null)
                {
                    Console.WriteLine(caja);
                    await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", "No se pudo obtener el rol");
                    return;
                }

                var dialog = new ContentDialog
                {
                    Title = "Confirmar",
                    Content = $"¿Desactivar la caja '{caja.Nombre}'?",
                    PrimaryButtonText = "Sí",
                    CloseButtonText = "Cancelar",
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result != ContentDialogResult.Primary)
                    return;

                Console.WriteLine($"ID enviado: {caja.CajaId}");
                await _cajaApiService.DesactivarCajaAsync(caja.CajaId);


                await CargarCajasAsync();

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
                if (_cajaSeleccionada == null)
                {
                    var dto = new CajaDto
                    {
                        Nombre = txtNombre.Text.Trim(),

                    };

                    await _cajaApiService.CrearCajaAsync(dto);
                }
                else
                {
                    var dto = new CajaDto
                    {
                        CajaId = _cajaSeleccionada.CajaId,
                        Nombre = txtNombre.Text.Trim()
                    };
                    await _cajaApiService.ActualizarCajaAsync(dto.CajaId, dto);

                }

                await CargarCajasAsync();
                LimpiarCampos();


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

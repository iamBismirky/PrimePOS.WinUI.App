using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CategoriaPage : Page
    {
        private CategoriaDto? _categoriaSeleccionada;
        private readonly CategoriaApiService _categoriaApiService;
        private List<CategoriaDto> _listaCategorias = new();
        private bool _isLoading;
        public CategoriaPage()
        {
            InitializeComponent();

            _categoriaApiService = App.AppServices.GetRequiredService<CategoriaApiService>();

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarCategoriasAsync();
        }


        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtBuscar.Focus(FocusState.Programmatic);
            OverlayCategoria.Visibility = Visibility.Collapsed;
            _categoriaSeleccionada = null;
        }
        private async Task CargarCategoriasAsync()
        {
            try
            {
                IsLoading = true;

                _listaCategorias = await _categoriaApiService.ObtenerCategoriasAsync();
                dgCategorias.ItemsSource = null;
                dgCategorias.ItemsSource = _listaCategorias;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
            finally { IsLoading = false; }

        }
        private void FiltrarCategorias()
        {
            var texto = txtBuscar.Text?.ToLower() ?? "";

            var filtrados = string.IsNullOrWhiteSpace(texto)
                ? _listaCategorias
                : _listaCategorias
                    .Where(r => r.Nombre.ToLower().Contains(texto))
                    .ToList();

            dgCategorias.ItemsSource = null;
            dgCategorias.ItemsSource = filtrados;
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
            txtTitulo.Text = "Crear Categoria";
            txtNombre.Focus(FocusState.Programmatic);
            OverlayCategoria.Visibility = Visibility.Visible;
        }
        private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
        {
            OverlayCategoria.Visibility = Visibility.Collapsed;
            LimpiarCampos();
        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitulo.Text = "Actualizar Categoria";
                txtNombre.Focus(FocusState.Programmatic);

                var btn = sender as Button;
                var categoria = btn?.Tag as CategoriaDto;

                if (categoria == null) return;

                _categoriaSeleccionada = categoria;


                txtNombre.Text = categoria.Nombre;


                OverlayCategoria.Visibility = Visibility.Visible;
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
                if (sender is Button btn && btn.Tag is CategoriaDto categoria)
                {

                    var dialog = new ContentDialog
                    {
                        Title = "Confirmar",
                        Content = $"¿Desactivar la categoria '{categoria.Nombre}'?",
                        PrimaryButtonText = "Sí",
                        CloseButtonText = "Cancelar",
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result != ContentDialogResult.Primary)
                        return;



                    await _categoriaApiService.DesactivarCategoriaAsync(categoria.CategoriaId);


                    await CargarCategoriasAsync();
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
                if (_categoriaSeleccionada == null)
                {
                    var dto = new CategoriaDto
                    {
                        Nombre = txtNombre.Text.Trim(),

                    };

                    await _categoriaApiService.CrearCategoriaAsync(dto);
                    await CargarCategoriasAsync();
                    LimpiarCampos();
                }
                else
                {
                    var dto = new CategoriaDto
                    {
                        CategoriaId = _categoriaSeleccionada.CategoriaId,
                        Nombre = txtNombre.Text.Trim()
                    };
                    await _categoriaApiService.ActualizarCategoriaAsync(dto.CategoriaId, dto);
                    await CargarCategoriasAsync();
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

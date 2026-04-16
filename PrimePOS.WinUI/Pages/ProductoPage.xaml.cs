using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class ProductoPage : Page
    {
        private ProductoDto? _productoSeleccionado;
        private readonly ProductoApiService _productoApiService;
        private readonly CategoriaApiService _categoriaApiService;
        private List<ProductoDto> _listaProductos = new();
        private List<CategoriaDto> _listaCategorias = new();

        private bool _isLoading;
        public ProductoPage()
        {
            InitializeComponent();

            _productoApiService = App.AppServices.GetRequiredService<ProductoApiService>();
            _categoriaApiService = App.AppServices.GetRequiredService<CategoriaApiService>();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarProductosAsync();
        }


        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtCodigoBarra.Text = "";
            ndPrecioCompra.Text = "";
            ndPrecioVenta.Text = "";
            ndExistencia.Text = "";
            txtFecha.Text = "";
            txtCodigo.Text = "";
            txtBuscar.Focus(FocusState.Programmatic);
            OverlayProducto.Visibility = Visibility.Collapsed;
            _productoSeleccionado = null;
        }
        private async Task CargarProductosAsync()
        {
            try
            {
                IsLoading = true;

                _listaProductos = await _productoApiService.ObtenerProductosAsync();
                dgProductos.ItemsSource = null;
                dgProductos.ItemsSource = _listaProductos;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
            finally { IsLoading = false; }

        }
        private async Task CargarCategoriasAsync()
        {
            try
            {
                IsLoading = true;

                _listaCategorias = await _categoriaApiService.ObtenerCategoriasAsync();
                cmbCategoria.ItemsSource = null;
                cmbCategoria.ItemsSource = _listaCategorias;
                cmbCategoria.DisplayMemberPath = "Nombre";
                cmbCategoria.SelectedValuePath = "CategoriaId";
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
                ? _listaProductos
                : _listaProductos
                    .Where(r => r.Nombre.ToLower().Contains(texto))
                    .ToList();

            dgProductos.ItemsSource = null;
            dgProductos.ItemsSource = filtrados;
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
            txtTitulo.Text = "Crear Producto";
            txtCodigo.Text = "------";
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtNombre.Focus(FocusState.Programmatic);
            OverlayProducto.Visibility = Visibility.Visible;
            await CargarCategoriasAsync();
        }
        private void CerrarOverlay_Click(object sender, RoutedEventArgs e)
        {
            OverlayProducto.Visibility = Visibility.Collapsed;
            LimpiarCampos();
        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitulo.Text = "Actualizar Cliente";
                txtNombre.Focus(FocusState.Programmatic);
                await CargarCategoriasAsync();
                var btn = sender as Button;
                var producto = btn?.Tag as ProductoDto;
                if (producto == null) return;

                _productoSeleccionado = producto;
                txtCodigo.Text = producto.Codigo.ToString();
                txtFecha.Text = producto.FechaRegistro.ToString("dd/MM/yyyy");
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion;
                cmbCategoria.SelectedValue = producto.CategoriaId;
                txtCodigoBarra.Text = producto.CodigoBarra;
                ndPrecioCompra.Text = producto.PrecioCompra.ToString();
                ndPrecioVenta.Text = producto.PrecioVenta.ToString();
                ndExistencia.Text = producto.Existencia.ToString();
                tsEstado.IsOn = producto.Estado;


                OverlayProducto.Visibility = Visibility.Visible;
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
                if (sender is Button btn && btn.Tag is ProductoDto producto)
                {

                    var dialog = new ContentDialog
                    {
                        Title = "Confirmar",
                        Content = $"¿Desactivar el producto '{producto.Nombre}'?",
                        CloseButtonText = "Cancelar",
                        PrimaryButtonText = "Sí",
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result != ContentDialogResult.Primary)
                        return;



                    await _productoApiService.DesactivarProductoAsync(producto.ProductoId);


                    await CargarProductosAsync();
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
                if (_productoSeleccionado == null)
                {
                    var dto = new CrearProductoDto
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        CodigoBarra = txtCodigoBarra.Text.Trim(),
                        CategoriaId = (int)(cmbCategoria.SelectedValue ?? 0),
                        PrecioCompra = decimal.Parse(ndPrecioCompra.Text.Trim()),
                        PrecioVenta = decimal.Parse(ndPrecioVenta.Text.Trim()),
                        Existencia = int.Parse(ndExistencia.Text.Trim()),
                        Estado = tsEstado.IsOn,

                    };

                    await _productoApiService.CrearProductoAsync(dto);
                    await CargarProductosAsync();
                    LimpiarCampos();
                }
                else
                {
                    var dto = new ActualizarProductoDto
                    {
                        ProductoId = _productoSeleccionado.ProductoId,
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        CodigoBarra = txtCodigoBarra.Text.Trim(),
                        CategoriaId = (int)(cmbCategoria.SelectedValue ?? 0),
                        PrecioCompra = decimal.Parse(ndPrecioCompra.Text.Trim()),
                        PrecioVenta = decimal.Parse(ndPrecioVenta.Text.Trim()),
                        Existencia = int.Parse(ndExistencia.Text.Trim()),
                        Estado = tsEstado.IsOn,
                    };
                    await _productoApiService.ActualizarProductoAsync(dto.ProductoId, dto);
                    await CargarProductosAsync();
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

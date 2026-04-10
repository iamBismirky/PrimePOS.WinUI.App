using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Overlays
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductoOverlay : UserControl
    {
        private readonly ProductoService _productoService;
        private readonly CategoriaService _categoriaService;

        public event Action? OnClose;

        public event Action<CrearProductoDto>? OnCrear;
        public event Action<ActualizarProductoDto>? OnActualizar;

        private bool _esEdicion = false;
        private int _productoId = 0;
        public ProductoOverlay()
        {
            this.InitializeComponent();
            _productoService = App.Services.GetRequiredService<ProductoService>();
            _categoriaService = App.Services.GetRequiredService<CategoriaService>();
            _ = CargarCategoriaAsync();

            NumberBoxHelper.AplicarFormatoMoneda(nbPrecioCompra);
            NumberBoxHelper.AplicarFormatoMoneda(nbPrecioVenta);

        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            OnClose?.Invoke();
        }
        private async Task CargarCategoriaAsync()
        {
            var lista = await _categoriaService.ListarCategoriasAsync();
            cmbCategoria.ItemsSource = lista;
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "CategoriaId";

        }
        public async Task SetData(ProductoDto dto)
        {
            await CargarCategoriaAsync();
            _esEdicion = true;
            _productoId = dto.ProductoId;

            txtNombre.Text = dto.Nombre;
            txtDescripcion.Text = dto.Descripcion;
            txtCodigoBarra.Text = dto.CodigoBarra;
            cmbCategoria.SelectedValue = dto.CategoriaId;
            nbPrecioCompra.Value = (double)dto.PrecioCompra;
            nbPrecioVenta.Value = (double)dto.PrecioVenta;
            nbExistencia.Value = dto.Existencia;
        }
        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_esEdicion)
                {
                    txtTitulo.Text = "Actualizar Producto";
                    var dto = new ActualizarProductoDto
                    {

                        CodigoBarra = txtCodigoBarra.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        PrecioCompra = (decimal)nbPrecioCompra.Value,
                        PrecioVenta = (decimal)nbPrecioVenta.Value,
                        Existencia = (int)nbExistencia.Value,

                    };
                    OnActualizar?.Invoke(dto);
                    OnClose?.Invoke();
                }
                else
                {
                    txtTitulo.Text = "Crear Producto";

                    var dto = new CrearProductoDto
                    {
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text,
                        CodigoBarra = txtCodigoBarra.Text,
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        PrecioCompra = (decimal)nbPrecioCompra.Value,
                        PrecioVenta = (decimal)nbPrecioVenta.Value,
                        Existencia = (int)nbExistencia.Value,
                    };

                    OnCrear?.Invoke(dto);
                    OnClose?.Invoke();

                }

            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
            }
        }

    }
}

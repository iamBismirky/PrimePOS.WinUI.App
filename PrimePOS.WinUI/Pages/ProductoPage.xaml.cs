using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages;


public sealed partial class ProductoPage : Page
{
    private int _productoIdSeleccionado = 0;
    public ProductoPage()
    {
        InitializeComponent();
    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        try
        {
            //Cargar listado de productos (DataGrid) y categorias(ComboBox)
            await ListarProductosAsync();
            await ListarCategoriasAsync();

            //Foco en el textBox Nombre
            txtNombre.Focus(FocusState.Programmatic);

            //Aplicar formato a los NumericBox decimales
            NumberBoxHelper.AplicarFormatoMoneda(nbPrecioCompra);
            NumberBoxHelper.AplicarFormatoMoneda(nbPrecioVenta);

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
            var dto = new CrearProductoDto
            {

                CodigoBarra = txtCodigoBarra.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                PrecioCompra = (decimal)nbPrecioCompra.Value,
                PrecioVenta = (decimal)nbPrecioVenta.Value,
                Existencia = (int)nbExistencia.Value,
                Estado = tsEstado.IsOn

            };
            await Servicios.ProductoService.CrearProductoAsync(dto);
            await ListarProductosAsync();
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
            var dto = new ActualizarProductoDto
            {
                ProductoId = _productoIdSeleccionado,
                CodigoBarra = txtCodigoBarra.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                PrecioCompra = (decimal)nbPrecioCompra.Value,
                PrecioVenta = (decimal)nbPrecioVenta.Value,
                Existencia = (int)nbExistencia.Value,
                Estado = tsEstado.IsOn

            };
            await Servicios.ProductoService.ActualizarProductoAsync(dto);
            await ListarProductosAsync();
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
            var dto = new EliminarProductoDto
            {
                ProductoId = _productoIdSeleccionado

            };
            await Servicios.ProductoService.EliminarProductoAsync(dto);
            await ListarProductosAsync();
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

    private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgProductos.SelectedItem is ProductoDto dto)
        {
            _productoIdSeleccionado = dto.ProductoId;
            txtCodigoBarra.Text = dto.CodigoBarra;
            txtNombre.Text = dto.Nombre;
            txtDescripcion.Text = dto.Descripcion;
            cmbCategoria.SelectedValue = dto.CategoriaId;
            nbPrecioCompra.Value = (double)(decimal)dto.PrecioCompra;
            nbPrecioVenta.Value = (double)(decimal)dto.PrecioVenta;
            nbExistencia.Value = (int)dto.Existencia;
            tsEstado.IsOn = dto.Estado;

        }

    }
    private async Task ListarProductosAsync()
    {
        try
        {
            var lista = await Servicios.ProductoService.ListarProductosAsync();
            dgProductos.ItemsSource = lista;
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private async Task ListarCategoriasAsync()
    {
        try
        {
            var lista = await Servicios.CategoriaService.ListarCategoriasAsync();
            cmbCategoria.ItemsSource = lista;

            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "CategoriaId";
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtCodigoBarra.Text = "";
        txtDescripcion.Text = "";
        cmbCategoria.SelectedIndex = -1;
        nbPrecioCompra.Value = 0;
        nbPrecioVenta.Value = 0;
        nbExistencia.Value = 0;
        tsEstado.IsOn = true;
        _productoIdSeleccionado = 0;
        dgProductos.SelectedItem = null;


    }
}


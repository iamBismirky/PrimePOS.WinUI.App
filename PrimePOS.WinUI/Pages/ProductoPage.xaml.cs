using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Foundation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProductoPage : Page
{
    public ProductoPage()
    {
        InitializeComponent();
    }
    private int _productoIdSeleccionado = 0;
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await ListarProductosAsync();
        await ListarCategoriasAsync();
    }
    private async void BtnCrear_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dto = new CrearProductoDto
            {
                Codigo = txtCodigo.Text,
                CodigoBarra = txtCodigoBarra.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                CategoriaId = (int)cmbCategoria.SelectedValue,
                PrecioCompra = (decimal)nbPrecioCompra.Value,
                PrecioVenta = (decimal)nbPrecioVenta.Value,
                Existencia = (int)nbPrecioVenta.Value,
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
                Codigo = txtCodigo.Text,
                CodigoBarra = txtCodigoBarra.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                CategoriaId = (int)cmbCategoria.SelectedValue,
                PrecioCompra = (decimal)nbPrecioCompra.Value,
                PrecioVenta = (decimal)nbPrecioVenta.Value,
                Existencia = (int)nbPrecioVenta.Value,
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
            txtCodigo.Text = dto.Codigo;
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
        txtCodigo.Text = "";
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


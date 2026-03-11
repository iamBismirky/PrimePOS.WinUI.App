using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Infrastructure;
using PrimePOS.WinUI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VentasPage : Page
{
    public VentasViewModel Vm { get; set; } = new();

    private DispatcherTimer _timer;

    public VentasPage()
    {
        InitializeComponent();
        DataContext = Vm;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(1000);

        _timer.Tick += async (s, e) =>
        {
            _timer.Stop();

            var productos = await Servicios.ProductoService
                .BuscarProductoCodigoONombreListAsync(txtBuscarProducto.Text);

            txtBuscarProducto.ItemsSource = productos;
        };

    }
    private void BtnBorrarProducto_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnGenerarFactura_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private async void BtnAgregarProductoCarrito_Click(object sender, RoutedEventArgs e)
    {
        var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

        if (producto != null)
        {
            Servicios.VentaService.AgregarProductoCarrito(producto);
            dgCarrito.ItemsSource = Servicios.VentaService.Carrito;
            
            txtSubtotal.Text = Servicios.VentaService.Subtotal.ToString("N2");
            txtImpuesto.Text = Servicios.VentaService.Impuesto.ToString("N2");
            txtTotal.Text = Servicios.VentaService.Total.ToString("N2");

            txtBuscarProducto.Text = "";
            txtBuscarProducto.Focus(FocusState.Programmatic);
        }


    }
    private async void txtBuscarProducto_DoubleClick(object sender, RoutedEventArgs e) { }
    
    private async void txtBuscarProducto_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

            if (producto != null)
            {

                Servicios.VentaService.AgregarProductoCarrito(producto);
                dgCarrito.ItemsSource = Servicios.VentaService.Carrito;

                txtSubtotal.Text = Servicios.VentaService.Subtotal.ToString("N2");
                txtImpuesto.Text = Servicios.VentaService.Impuesto.ToString("N2");
                txtTotal.Text = Servicios.VentaService.Total.ToString("N2");

                txtBuscarProducto.Text = "";
                txtBuscarProducto.Focus(FocusState.Programmatic);
            }
        }

    }
    private async void txtBuscarProducto_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (txtBuscarProducto.Text.Length < 3)
        {
            txtBuscarProducto.ItemsSource = null;
            return;
        }
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _timer.Stop();
                _timer.Start();
            }
        
    }

    private async void txtBuscarProducto_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

        {
            var texto = sender.Text;

            var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(texto);

            if (producto != null)
            {
                Servicios.VentaService.AgregarProductoCarrito(producto);

                dgCarrito.ItemsSource = Servicios.VentaService.Carrito;

                CalcularTotales();

                sender.Text = "";
                sender.Focus(FocusState.Programmatic);
            }
        }
    }
    private void txtBuscarProducto_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var producto = (ProductoDto)args.SelectedItem;

        sender.Text = producto.Nombre + producto.Codigo;
    }
    private void CalcularTotales()
    {
        txtSubtotal.Text = Servicios.VentaService.Subtotal.ToString("N2");
        txtImpuesto.Text = Servicios.VentaService.Impuesto.ToString("N2");
        txtTotal.Text = Servicios.VentaService.Total.ToString("N2");
    }

}

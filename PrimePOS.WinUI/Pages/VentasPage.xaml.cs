using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages;


public sealed partial class VentasPage : Page
{

    private DispatcherTimer _timerProducto = new DispatcherTimer();
    private DispatcherTimer _timerCliente = new DispatcherTimer();
    private ClienteDto? _clienteSeleccionado;
    private ProductoDto? _productoSeleccionado;

    public VentasPage()
    {
        InitializeComponent();
        InicializarBuscador();
        txtFecha.Text = DateTime.Now.ToString("dd/MM/yyy");
        NavigationCacheMode = NavigationCacheMode.Required;




    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);


    }
    private async void Page_loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await ListarClientes();
            await ListarMetodosPagos();
            await CargarConsumidorFinal();
            CalcularTotales();
            txtBuscarProducto.Focus(FocusState.Programmatic);
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private void BtnEliminarProducto_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = button?.DataContext as CarritoItem;

        if (item == null)
            return;

        Servicios.VentaService.EliminarProducto(item.ProductoId);
        CalcularTotales();
        txtBuscarProducto.Focus(FocusState.Programmatic);

    }
    private void BtnBorrarProducto_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
    private void BtnGenerarFactura_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
    private async void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        LimpiarCampos();
        txtBuscarProducto.Focus(FocusState.Programmatic);
    }
    private async void BtnAgregarProductoCarrito_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var producto = await Servicios.ProductoService
                    .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

            if (producto != null)
            {
                await Servicios.VentaService.AgregarProductoCarrito(producto.ProductoId);
                dgCarrito.ItemsSource = Servicios.VentaService.Carrito;

                txtSubtotal.Text = $"Subtotal: ${Servicios.VentaService.Subtotal.ToString("N2")}";
                txtImpuesto.Text = $"Impuesto: ${Servicios.VentaService.Impuesto.ToString("N2")}";
                txtTotal.Text = $"Total: ${Servicios.VentaService.Total.ToString("N2")}";

                txtBuscarProducto.Text = "";
                txtBuscarProducto.Focus(FocusState.Programmatic);
            }
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }


    }
    private async void txtBuscarProducto_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        if (string.IsNullOrWhiteSpace(sender.Text) || sender.Text.Length < 2)
        {
            sender.ItemsSource = new List<ProductoDto>();
            _timerProducto.Stop();
            return;
        }

        _timerProducto.Stop();
        _timerProducto.Start();



    }
    private async void txtBuscarCliente_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {

        if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        if (string.IsNullOrWhiteSpace(sender.Text) || sender.Text.Length < 2)
        {
            sender.ItemsSource = new List<ClienteDto>();
            _timerCliente.Stop();
            return;
        }

        _timerCliente.Stop();
        _timerCliente.Start();




    }
    private async void txtBuscarProducto_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        try
        {
            ProductoDto? producto = null;

            // ✅ 1. Si el usuario eligió de la lista (flechas + enter o click)
            if (args.ChosenSuggestion != null)
            {
                producto = (ProductoDto)args.ChosenSuggestion;
            }
            else
            {
                // ⚠️ 2. Si escribió manual (ej: código de barras)
                var texto = sender.Text;

                if (string.IsNullOrEmpty(texto))
                    return;

                producto = await Servicios.ProductoService
                    .BuscarProductoCodigoONombreAsync(texto);
            }

            if (producto != null)
            {
                await Servicios.VentaService.AgregarProductoCarrito(producto.ProductoId);

                dgCarrito.ItemsSource = Servicios.VentaService.Carrito;

                CalcularTotales();

                sender.Text = "";
                _productoSeleccionado = null;
                sender.Focus(FocusState.Programmatic);
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }

    }
    private async void txtBuscarCliente_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

        try
        {

            var texto = sender.Text;

            if (string.IsNullOrEmpty(texto))
                return;

            var cliente = await Servicios.ClienteService
                .BuscarClienteCodigoONombreAsync(texto);
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }

    }
    private void txtBuscarProducto_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {

        _productoSeleccionado = (ProductoDto)args.SelectedItem;

        sender.Text = _productoSeleccionado.Nombre;
    }
    private async void txtBuscarCliente_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {

        var clienteSeleccionado = (ClienteDto)args.SelectedItem;

        sender.Text = clienteSeleccionado.Nombre;
    }
    private void CalcularTotales()
    {
        txtSubtotal.Text = Servicios.VentaService.Subtotal.ToString("N2");
        txtDescuento.Text = Servicios.VentaService.DescuentoMonto.ToString("N2");
        txtImpuesto.Text = Servicios.VentaService.Impuesto.ToString("N2");
        txtTotal.Text = Servicios.VentaService.Total.ToString("N2");
    }
    private async Task ListarClientes()
    {
        //try
        //{
        //    var lista = await Servicios.ClienteService.ListarClientes();

        //    cmbCliente.ItemsSource = lista;

        //    cmbCliente.DisplayMemberPath = "Nombre";
        //    cmbCliente.SelectedValuePath = "ClienteId";
        //    cmbCliente.SelectedIndex = 0;
        //}
        //catch (Exception ex)
        //{
        //    await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        //}

    }
    private async Task ListarMetodosPagos()
    {
        var lista = await Servicios.MetodoPagoService.ListarMetodosPagosAsync();

        cmbMetodoPago.ItemsSource = lista;

        cmbMetodoPago.DisplayMemberPath = "Nombre";
        cmbMetodoPago.SelectedValuePath = "MetodoPagoId";
        cmbMetodoPago.SelectedIndex = 0;


    }
    private void InicializarBuscador()
    {
        _timerProducto = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _timerProducto.Tick += TimerBuscarProducto_Tick;

        _timerCliente = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _timerCliente.Tick += TimerBuscarCliente_Tick;
    }
    private async void TimerBuscarProducto_Tick(object? sender, object e)
    {
        try
        {
            _timerProducto.Stop();

            await BuscarProductosAsync();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private async void TimerBuscarCliente_Tick(object? sender, object e)
    {
        try
        {
            _timerCliente.Stop();

            await BuscarClientesAsync();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }
    }
    private async Task BuscarProductosAsync()
    {
        try
        {


            var productos = await Servicios.ProductoService
                .BuscarProductoCodigoONombreListAsync(txtBuscarProducto.Text);


            txtBuscarProducto.ItemsSource = productos;

        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private async Task BuscarClientesAsync()
    {

        try
        {
            var cliente = await Servicios.ClienteService
            .BuscarClienteCodigoONombreListAsync(txtBuscarCliente.Text);

            txtBuscarCliente.ItemsSource = cliente;
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private async Task CargarConsumidorFinal()
    {
        try
        {
            var cliente = await Servicios.ClienteService.ObtenerPorId(1);
            if (cliente != null)
            {
                _clienteSeleccionado = cliente;
                txtBuscarCliente.Text = cliente.Nombre;
            }
        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

        }
    }
    private async void LimpiarCampos()
    {
        txtBuscarProducto.Text = "";

        dgCarrito.ItemsSource = null;
        Servicios.VentaService.VaciarCarrito();

        CalcularTotales();

        txtBuscarProducto.Focus(FocusState.Programmatic);
        await CargarConsumidorFinal();


    }
    private void cmbDescuento_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        decimal porcentaje = Convert.ToDecimal(cmbDescuento.SelectedValue);

        Servicios.VentaService.AplicarDescuento(porcentaje);
        CalcularTotales();


    }

}

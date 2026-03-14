using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages;


public sealed partial class VentasPage : Page
{

    private DispatcherTimer _timer = new DispatcherTimer();
    private ClienteDto? clienteSeleccionado;

    public VentasPage()
    {
        InitializeComponent();
        InicializarBuscador();


        NavigationCacheMode = NavigationCacheMode.Required;


    }
    private async void Page_loaded(object sender, RoutedEventArgs e)
    {
        await ListarClientes();
        await ListarMetodosPagos();
        await CargarConsumidorFinal();
        txtBuscarProducto.Focus(FocusState.Programmatic);
    }
    private void BtnEliminarProducto_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = button?.DataContext as CarritoItem;

        if (item == null)
            return;

        Servicios.VentaService.EliminarProducto(item.ProductoId);

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

    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        LimpiarCampos();
        txtBuscarProducto.Focus(FocusState.Programmatic);
    }
    private async void BtnAgregarProductoCarrito_Click(object sender, RoutedEventArgs e)
    {
        var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

        if (producto != null)
        {
            Servicios.VentaService.AgregarProductoCarrito(producto);
            dgCarrito.ItemsSource = Servicios.VentaService.Carrito;

            txtSubtotal.Text = $"Subtotal: ${Servicios.VentaService.Subtotal.ToString("N2")}";
            txtImpuesto.Text = $"Impuesto: ${Servicios.VentaService.Impuesto.ToString("N2")}";
            txtTotal.Text = $"Total: ${Servicios.VentaService.Total.ToString("N2")}";

            txtBuscarProducto.Text = "";
            txtBuscarProducto.Focus(FocusState.Programmatic);
        }


    }

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
        if (string.IsNullOrWhiteSpace(sender.Text) || sender.Text.Length < 3)
        {
            sender.ItemsSource = null;
            _timer?.Stop();
            return;
        }

        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            _timer?.Stop();
            _timer?.Start();
        }


    }
    private async void txtBuscarCliente_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(sender.Text) || sender.Text.Length < 3)
        {
            sender.ItemsSource = null;
            _timer?.Stop();
            return;
        }

        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            _timer?.Stop();
            _timer?.Start();
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
    private async void txtBuscarCliente_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

        {
            var texto = sender.Text;

            var cliente = await Servicios.ClienteService
                .BuscarClienteCodigoONombreListAsync(texto);


        }
    }
    private void txtBuscarProducto_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var producto = (ProductoDto)args.SelectedItem;

        sender.Text = producto.Nombre;
    }
    private void txtBuscarCliente_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var cliente = (ClienteDto)args.SelectedItem;

        sender.Text = cliente.Nombre;
    }
    private void CalcularTotales()
    {
        txtSubtotal.Text = Servicios.VentaService.Subtotal.ToString("N2");
        txtImpuesto.Text = Servicios.VentaService.Impuesto.ToString("N2");
        txtTotal.Text = Servicios.VentaService.Total.ToString("N2");
    }
    private async Task ListarClientes()
    {
        var lista = await Servicios.ClienteService.ListarClientes();

        cmbCliente.ItemsSource = lista;

        cmbCliente.DisplayMemberPath = "Nombre";
        cmbCliente.SelectedValuePath = "ClienteId";
        cmbCliente.SelectedIndex = 0;

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
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };

        _timer.Tick += TimerBuscar_Tick;
    }
    private async Task BuscarProductosAsync()
    {
        if (string.IsNullOrWhiteSpace(txtBuscarProducto.Text) || txtBuscarProducto.Text.Length < 3)
        {
            txtBuscarProducto.ItemsSource = null;
            return;
        }

        var productos = await Servicios.ProductoService
            .BuscarProductoCodigoONombreListAsync(txtBuscarProducto.Text);

        txtBuscarProducto.ItemsSource = productos;
    }
    private async Task BuscarClientesAsync()
    {
        if (string.IsNullOrWhiteSpace(txtBuscarCliente.Text) || txtBuscarCliente.Text.Length < 3)
        {
            txtBuscarCliente.ItemsSource = null;
            return;
        }

        var clientes = await Servicios.ClienteService
            .BuscarClienteCodigoONombreListAsync(txtBuscarCliente.Text);

        txtBuscarCliente.ItemsSource = clientes;
    }
    private async void TimerBuscar_Tick(object? sender, object e)
    {
        try
        {
            _timer.Stop();
            await BuscarProductosAsync();
            await BuscarClientesAsync();


        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }

    }
    private async Task CargarConsumidorFinal()
    {
        var cliente = await Servicios.ClienteService.ObtenerPorId(1);
        if (cliente != null)
        {
            clienteSeleccionado = cliente;
            txtBuscarCliente.Text = cliente.Nombre;
        }
    }
    private void LimpiarCampos()
    {
        txtBuscarProducto.Text = "";

        dgCarrito.ItemsSource = null;
        Servicios.VentaService.VaciarCarrito();

        txtSubtotal.Text = null;
        txtImpuesto.Text = null;
        txtTotal.Text = null;

        txtBuscarProducto.Focus(FocusState.Programmatic);



    }
}

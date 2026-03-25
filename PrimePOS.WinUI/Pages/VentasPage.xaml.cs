using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.BLL.Services;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace PrimePOS.WinUI.Pages;


public sealed partial class VentasPage : Page
{
    private readonly VentaService _ventaService;
    private readonly TurnoService _turnoService;
    private readonly CajaService _cajaService;
    private readonly ProductoService _productoService;
    private readonly ClienteService _clienteService;
    private readonly MetodoPagoService _metodoPagoService;

    private DispatcherTimer _timerProducto = new DispatcherTimer();
    private DispatcherTimer _timerCliente = new DispatcherTimer();
    private ClienteDto? _clienteSeleccionado;
    private ProductoDto? _productoSeleccionado;

    public Caja? _cajaSeleccionada { get; private set; }
    public Turno? _turnoSeleccionado { get; private set; }
    public string? TextoTurnoPreview { get; set; }

    public VentasPage()
    {
        this.InitializeComponent();
        InicializarBuscador();
        NavigationCacheMode = NavigationCacheMode.Required;

        _ventaService = App.Services.GetRequiredService<VentaService>();
        _turnoService = App.Services.GetRequiredService<TurnoService>();
        _cajaService = App.Services.GetRequiredService<CajaService>();
        _productoService = App.Services.GetRequiredService<ProductoService>();
        _clienteService = App.Services.GetRequiredService<ClienteService>();
        _metodoPagoService = App.Services.GetRequiredService<MetodoPagoService>();


    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await ListarMetodosPagosAsync();

        Sesion.TurnoActual = await _turnoService.ObtenerTurnoAbiertoPorCajaAsync(Sesion.CajaId);

        if (Sesion.TurnoActual != null)
        {
            txtCaja.Text = Sesion.TurnoActual.CajaId.ToString();
            txtUsuario.Text = Sesion.UsuarioNombre.ToString();
            txtRol.Text = Sesion.RolNombre.ToString();
            txtTurno.Text = Sesion.TurnoActual.TurnoId.ToString();
            txtFecha.Text = Sesion.TurnoActual.FechaApertura.ToString();
        }
        else
        {
            TurnoOverlay.Visibility = Visibility.Visible;
        }

    }

    private async void Page_loaded(object sender, RoutedEventArgs e)
    {

        try
        {

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

        _ventaService.EliminarProducto(item.ProductoId);
        CalcularTotales();
        txtBuscarProducto.Focus(FocusState.Programmatic);

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

            // Si el usuario eligió de la lista (flechas + enter o click)
            if (args.ChosenSuggestion != null)
            {
                producto = (ProductoDto)args.ChosenSuggestion;
            }
            else
            {
                // Si escribió manual (ej: código de barras)
                var texto = sender.Text;

                if (string.IsNullOrEmpty(texto))
                    return;

                producto = await _productoService
                    .BuscarProductoCodigoONombreAsync(texto);
            }

            if (producto != null)
            {
                await _ventaService.AgregarProductoCarrito(producto.ProductoId);

                dgCarrito.ItemsSource = _ventaService.Carrito;

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
            ClienteDto? cliente = null;


            if (args.ChosenSuggestion != null)
            {
                cliente = (ClienteDto)args.ChosenSuggestion;
            }
            else
            {

                var texto = sender.Text;

                if (string.IsNullOrEmpty(texto))
                    return;

                cliente = await _clienteService
                    .BuscarClienteCodigoONombreAsync(texto);
            }
            if (cliente != null)
            {
                txtCodigoCliente.Text = cliente.Codigo;
                txtNombreCliente.Text = cliente.Nombre;
                txtDocumentoCliente.Text = cliente.Documento;
                txtBuscarCliente.Text = "";
            }

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
        sender.Text = "";
    }
    private async void txtBuscarCliente_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {

        var clienteSeleccionado = (ClienteDto)args.SelectedItem;

        sender.Text = clienteSeleccionado.Nombre;
        sender.Text = "";
    }
    private void CalcularTotales()
    {
        txtSubtotal.Text = _ventaService.Subtotal.ToString("N2");
        txtDescuento.Text = _ventaService.DescuentoMonto.ToString("N2");
        txtImpuesto.Text = _ventaService.Impuesto.ToString("N2");
        txtTotal.Text = _ventaService.Total.ToString("N2");
    }
    private async Task ListarMetodosPagosAsync()
    {
        try
        {
            var lista = await _metodoPagoService.ListarMetodosPagosAsync();

            cmbMetodoPago.ItemsSource = null;
            cmbMetodoPago.ItemsSource = lista;

            cmbMetodoPago.DisplayMemberPath = "Nombre";
            cmbMetodoPago.SelectedValuePath = "MetodoPagoId";
            cmbMetodoPago.SelectedIndex = 0;
        }
        catch (Exception ex)
        {

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());

        }


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
            var productos = await _productoService.BuscarProductoCodigoONombreListAsync(txtBuscarProducto.Text);
            txtBuscarProducto.ItemsSource = productos;
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
            var cliente = await _clienteService
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
            var cliente = await _clienteService.ObtenerPorId(1);
            if (cliente != null)
            {
                _clienteSeleccionado = cliente;
                txtCodigoCliente.Text = cliente.Codigo;
                txtNombreCliente.Text = cliente.Nombre;
                txtDocumentoCliente.Text = cliente.Documento;
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
        _ventaService.VaciarCarrito();
        CalcularTotales();
        txtBuscarCliente.Text = "";
        txtBuscarProducto.Focus(FocusState.Programmatic);
        await CargarConsumidorFinal();


    }
    private void cmbDescuento_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        decimal porcentaje = Convert.ToDecimal(cmbDescuento.SelectedValue);
        if (porcentaje != 0)
        {
            _ventaService.AplicarDescuento(porcentaje);
            CalcularTotales();

        }


    }
    private async void BtnAbrirTurno_Click(object sender, RoutedEventArgs e)
    {
        if (Sesion.TurnoActual == null)
        {
            TurnoOverlay.Visibility = Visibility.Visible;

        }
        await DialogHelper.MostrarMensaje(this.XamlRoot, "Adventencia", "Existe un turno abierto");
    }
    private async void BtnCerrarTurno_Click(object sender, RoutedEventArgs e)
    {
        TurnoCerrarOverlay.Visibility = Visibility.Visible;
    }


}

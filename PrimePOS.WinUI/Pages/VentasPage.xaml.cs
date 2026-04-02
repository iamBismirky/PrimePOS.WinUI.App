using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.Services;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using PrimePOS.WinUI.Overlays;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    private ClienteDto? _clienteDto;
    private ProductoDto? _productoDto;

    public Caja? _cajaSeleccionada { get; private set; }
    public Turno? _turnoSeleccionado { get; private set; }
    public string? TextoTurnoPreview { get; set; }

    public VentaViewModel _ventaViewModel { get; set; }
    private IServiceScope _scope;
    public VentasPage()
    {
        InitializeComponent();
        InicializarBuscador();
        //NavigationCacheMode = NavigationCacheMode.Required;

        _scope = App.Services.CreateScope();

        _ventaService = _scope.ServiceProvider.GetRequiredService<VentaService>();
        _turnoService = _scope.ServiceProvider.GetRequiredService<TurnoService>();
        _cajaService = _scope.ServiceProvider.GetRequiredService<CajaService>();
        _productoService = _scope.ServiceProvider.GetRequiredService<ProductoService>();
        _clienteService = _scope.ServiceProvider.GetRequiredService<ClienteService>();
        _metodoPagoService = _scope.ServiceProvider.GetRequiredService<MetodoPagoService>();

        _ventaViewModel = App.Services.GetRequiredService<VentaViewModel>();
        this.DataContext = _ventaViewModel;


    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        _ventaViewModel.AppSesion.PropertyChanged += Sesion_PropertyChanged;

        VerificarTurno();



    }

    private async void Page_loaded(object sender, RoutedEventArgs e)
    {


        try
        {

            await CargarConsumidorFinal();
            await ListarMetodosPagosAsync();

            txtBuscarProducto.Focus(FocusState.Programmatic);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());
        }



    }
    private void BtnEliminarProducto_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = button?.DataContext as CarritoItemViewModel;

        if (item == null)
            return;

        _ventaViewModel.EliminarProductoCarrito(item.ProductoId);

        txtBuscarProducto.Focus(FocusState.Programmatic);

    }
    private async void BtnGenerarFactura_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _ventaViewModel.FacturarAsync(_clienteDto!.ClienteId, (int)cmbMetodoPago.SelectedValue);

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Éxito", "Venta realizada correctamente");


        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());
            System.Diagnostics.Debug.WriteLine(ex);
        }


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
            if (producto == null || producto.ProductoId <= 0)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", "Producto no encontrado");
                return;
            }


            await _ventaViewModel.AgregarProductoCarrito(producto);



            sender.Text = "";
            _productoDto = null;
            sender.Focus(FocusState.Programmatic);

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
    private async void txtBuscarProducto_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {

        _productoDto = (ProductoDto)args.SelectedItem;

        sender.Text = _productoDto.Nombre;
    }
    private async void txtBuscarCliente_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {

        var clienteSeleccionado = (ClienteDto)args.SelectedItem;

        sender.Text = clienteSeleccionado.Nombre;
        sender.Text = "";
    }

    private async Task ListarMetodosPagosAsync()
    {
        try
        {
            var lista = await _metodoPagoService.ListarMetodosPagosAsync();


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
                _clienteDto = cliente;
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



    }
    private void cmbDescuento_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (cmbDescuento.SelectedItem is ComboBoxItem item)
        {
            var porcentaje = Convert.ToDecimal(item.Tag);

            if (_ventaViewModel != null)
            {
                _ventaViewModel.AplicarDescuento(porcentaje);

            }





        }



    }
    private async void BtnAbrirTurno_Click(object sender, RoutedEventArgs e)
    {
        if (_ventaViewModel.AppSesion.TurnoActual != null)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", "Hay un turno abierto");
            return;
        }
        MostrarOverlayAbrirTurno();
    }
    private async void BtnCerrarTurno_Click(object sender, RoutedEventArgs e)
    {
        if (_ventaViewModel.AppSesion.TurnoActual == null)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", "No hay turno abierto");
            return;
        }

        MostrarOverlayCerrarTurno();

    }
    private void MostrarOverlayAbrirTurno()
    {
        var overlay = new AbrirTurnoOverlay();
        overlay.OnCerrar += () =>
        {
            CerrarOverlay();
        };
        MostrarOverlay(overlay);

    }
    private void MostrarOverlayCerrarTurno()
    {
        var overlay = new CerrarTurnoOverlay();
        overlay.OnCerrar += () =>
        {
            CerrarOverlay();
        };
        MostrarOverlay(overlay);
    }
    public void CerrarOverlay()
    {
        OverlayContent.Content = null;
        OverlayContainer.Visibility = Visibility.Collapsed;
    }
    private void MostrarOverlay(UserControl overlay)
    {
        OverlayContent.Content = overlay;
        OverlayContainer.Visibility = Visibility.Visible;
    }
    private async void VerificarTurno()
    {
        if (_ventaViewModel.AppSesion.HayTurnoAbierto)
        {
            MostrarOverlayAbrirTurno();

        }
        else
        {
            var turno = await _turnoService.ObtenerTurnoAbiertoPorCajaAsync(Sesion.CajaId);

            if (turno != null)
            {
                _ventaViewModel.AppSesion.TurnoActual = turno;
            }
        }


    }
    private void Sesion_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SesionService.TurnoActual))
        {
            if (!_ventaViewModel.AppSesion.HayTurnoAbierto)
            {
                MostrarOverlayAbrirTurno();
            }
        }
    }
}

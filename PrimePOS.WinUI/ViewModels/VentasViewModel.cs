using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.WinUI.Overlays;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class VentaViewModel : ObservableObject
{
    private readonly ProductoApiService _productoApi;
    private readonly ClienteApiService _clienteApi;
    private readonly MetodoPagoApiService _metodoPagoApi;
    private readonly CajaApiService _cajaApi;
    private readonly TurnoApiService _turnoApi;
    private readonly VentaApiService _ventaApi;
    private readonly FacturaApiService _facturaApi;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;
    private readonly OverlayService _overlayService;
    public AppSesionViewModel AppSesion => _sesion;

    public VentaViewModel(
        ProductoApiService productoApi,
        ClienteApiService clienteApi,
        MetodoPagoApiService metodoPagoApi,
        CajaApiService cajaApi,
        TurnoApiService turnoApi,
        VentaApiService ventaApi,
        FacturaApiService facturaApi,
        NotificationService notify,
        AppSesionViewModel sesion,
        OverlayService overlay)
    {
        _productoApi = productoApi;
        _clienteApi = clienteApi;
        _metodoPagoApi = metodoPagoApi;
        _cajaApi = cajaApi;
        _turnoApi = turnoApi;
        _ventaApi = ventaApi;
        _facturaApi = facturaApi;
        _notify = notify;
        _sesion = sesion;
        _overlayService = overlay;
    }

    // =========================
    // 🔥 OVERLAYS
    // =========================
    public Action<object>? MostrarOverlay;
    public Action? CerrarOverlay;

    // =========================
    // 🔹 PROPIEDADES
    // =========================

    [ObservableProperty] private string textoProducto = "";
    [ObservableProperty] private string textoCliente = "";

    [ObservableProperty] private ObservableCollection<ProductoDto> productos = new();
    [ObservableProperty] private ObservableCollection<ClienteDto> clientes = new();


    [ObservableProperty] private ObservableCollection<MetodoPagoDto> metodosPago = new();
    [ObservableProperty] private MetodoPagoDto? metodoPagoSeleccionado;

    [ObservableProperty] private ClienteDto? clienteSeleccionado;

    [ObservableProperty] private ObservableCollection<CarritoItemViewModel> carrito = new();

    [ObservableProperty] private decimal descuentoPorcentaje;
    [ObservableProperty] private decimal descuentoSeleccionado;
    [ObservableProperty] public bool isLoading;

    // =========================
    // 🔹 TOTALES
    // =========================

    public decimal Subtotal => Carrito.Sum(x => x.Total);
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Impuesto => SubtotalConDescuento * 0.18m;
    public decimal Total => Subtotal + Impuesto - DescuentoMonto;
    public ObservableCollection<Decimal> Descuentos { get; } = new()
    { 0,5,10,15,20,25,30};
    partial void OnDescuentoSeleccionadoChanged(decimal value)
    {
        DescuentoPorcentaje = value;
        NotificarTotales();
    }
    partial void OnDescuentoPorcentajeChanged(decimal value)
    {
        NotificarTotales();
    }

    private void NotificarTotales()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(DescuentoMonto));
        OnPropertyChanged(nameof(SubtotalConDescuento));
        OnPropertyChanged(nameof(Impuesto));
        OnPropertyChanged(nameof(Total));
    }

    // =========================
    // 🔹 INIT
    // =========================

    public async Task InicializarAsync()
    {
        await CargarMetodosPagoAsync();
        await CargarConsumidorFinalAsync();
        await VerificarTurnoAsync();
    }

    // =========================
    // 🔹 TURNO
    // =========================

    private async Task VerificarTurnoAsync()
    {
        var res = await _turnoApi.ObtenerTurnoActivoAsync();

        if (res.Success && res.Data != null)
        {
            _sesion.TurnoActual = res.Data;
        }

        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning("Debe abrir un turno");
            await AbrirTurnoAsync();
        }
    }

    // =========================
    // 🔹 CLIENTE DEFAULT
    // =========================

    private async Task CargarConsumidorFinalAsync()
    {
        var res = await _clienteApi.ObtenerClientePorIdAsync(1);

        if (res.Success && res.Data != null)
        {
            ClienteSeleccionado = res.Data;
        }
    }

    private async Task CargarMetodosPagoAsync()
    {
        var res = await _metodoPagoApi.ObtenerMetodosPagoAsync();

        if (!res.Success || res.Data == null)
        {
            _notify.Warning(res.Message);
            return;
        }

        MetodosPago = new ObservableCollection<MetodoPagoDto>(res.Data);
        MetodoPagoSeleccionado = MetodosPago.FirstOrDefault();
    }


    [RelayCommand]
    public async Task BuscarProductosAsync()
    {
        try
        {
            // 🔹 evitar llamadas innecesarias
            if (string.IsNullOrWhiteSpace(TextoProducto) || TextoProducto.Length < 2)
            {
                Productos.Clear();
                return;
            }

            var res = await _productoApi.BuscarProductosAsync(TextoProducto);

            // patrón correcto con ApiResponse
            if (!res.Success)
            {
                _notify.Warning(res.Message);

                return;
            }

            // evitar null
            Productos = new ObservableCollection<ProductoDto>(
                res.Data ?? new List<ProductoDto>()
            );
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
    }

    [RelayCommand]
    public async Task SeleccionarProductoAsync(ProductoDto producto)
    {
        if (producto == null) return;

        await AgregarProductoCarritoAsync(producto);

        TextoProducto = "";
        Productos.Clear();
    }

    // =========================
    // 🔹 BUSCAR CLIENTES
    // =========================

    [RelayCommand]
    public async Task BuscarClientesAsync()
    {
        if (TextoCliente.Length < 2)
        {
            Clientes.Clear();
            return;
        }

        var res = await _clienteApi.BuscarClientesAsync(TextoCliente);

        if (res.Success && res.Data != null)
            Clientes = new ObservableCollection<ClienteDto>(res.Data);
    }

    [RelayCommand]
    public void SeleccionarCliente(ClienteDto cliente)
    {
        ClienteSeleccionado = cliente;
        TextoCliente = "";
    }

    // =========================
    // 🔹 CARRITO
    // =========================

    public async Task AgregarProductoCarritoAsync(ProductoDto dto)
    {

        var existente = Carrito.FirstOrDefault(p => p.ProductoId == dto.ProductoId);
        if (dto.Existencia <= 0)
        {
            _notify.Warning($"No hay stock disponible de {dto.Nombre}");
            return;
        }
        if (dto.Existencia <= 5)
        {
            _notify.Warning($"Inventario bajo de {dto.Nombre} - Existen {dto.Existencia}");
        }

        if (existente != null)
            existente.Cantidad++;
        else
            Carrito.Add(new CarritoItemViewModel
            {
                Codigo = dto.Codigo,
                ProductoId = dto.ProductoId,
                Nombre = dto.Nombre,
                Precio = dto.PrecioVenta,
                Cantidad = 1
            });

        NotificarTotales();
    }

    [RelayCommand]
    public void EliminarProducto(CarritoItemViewModel item)
    {
        if (item == null) return;

        Carrito.Remove(item);
        NotificarTotales();
    }

    [RelayCommand]
    private void Limpiar()
    {
        Carrito.Clear();
        DescuentoPorcentaje = 0;
        NotificarTotales();
    }

    // =========================
    // 💰 COBRAR (ABRE OVERLAY)
    // =========================

    [RelayCommand]
    private async Task AbrirTurnoAsync()
    {
        //var vm = new AbrirTurnoViewModel(
        //    _cajaApi,
        //    _turnoApi,
        //    _notify,
        //    _sesion
        //);

        //await vm.InicializarAsync();


        //MostrarOverlay?.Invoke(vm);
        //if (_sesion.HayTurnoAbierto == false)
        //{
        //    vm.OnCerrar += () => CerrarOverlay?.Invoke();
        //    _notify.Warning("Debe abrir un turno para continuar");
        //}
        var vm = new AbrirTurnoViewModel(
        _cajaApi,
        _turnoApi,
        _notify,
        _sesion);

        await vm.InicializarAsync();

        // SOLO cuando el turno abra correctamente
        vm.OnTurnoAbierto += () =>
       {
           CerrarOverlay?.Invoke();
       };

        MostrarOverlay?.Invoke(vm);

        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning(
                "Debe abrir un turno para continuar");
        }
    }
    [RelayCommand]
    private async Task CobrarAsync()
    {
        if (!Carrito.Any())
        {
            _notify.Warning("No hay productos");
            return;
        }

        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning("Debe abrir un turno");
            return;
        }

        var vm = new CobrarViewModel(Total);
        MostrarOverlay?.Invoke(vm);
        vm.ConfirmadoAsync += async (cobro) =>
        {
            var result = await FacturarDesdeCobroAsync(cobro.Efectivo);

            if (result == null || !result.Success || result.Data == null)
            {
                _notify.Warning(result!.Message);
                return;
            }


            // cerrar cobrar
            CerrarOverlay?.Invoke();

            // abrir factura
            MostrarOverlay?.Invoke(new FacturaViewModel(result.Data.UrlPdf));
        };


    }
    [RelayCommand]
    public async Task CerrarTurnoAsync()
    {
        var vm = new CerrarTurnoViewModel(
            _turnoApi,
            _notify,
            _sesion
        );
        await vm.InicializarAsync();
        vm.OnCerrar += () => CerrarOverlay?.Invoke();
        MostrarOverlay?.Invoke(vm);
    }

    // =========================
    // 🧾 FACTURAR DESDE COBRO
    // =========================

    public async Task<ApiResponse<VentaResponseDto>?> FacturarDesdeCobroAsync(decimal efectivo)
    {
        try
        {
            if (MetodoPagoSeleccionado == null)
            {
                _notify.Warning("Seleccione un método de pago");
                return null;
            }

            if (ClienteSeleccionado == null)
            {
                _notify.Warning("Seleccione un cliente");
                return null;
            }

            var dto = new CrearVentaDto
            {
                ClienteId = ClienteSeleccionado.ClienteId,
                ClienteNombre = ClienteSeleccionado.Nombre,
                MetodoPagoId = MetodoPagoSeleccionado.MetodoPagoId,
                TurnoId = _sesion.TurnoActual!.TurnoId,

                Subtotal = Subtotal,
                Impuesto = Impuesto,
                Descuento = DescuentoMonto,
                Total = Total,
                Efectivo = efectivo,

                Items = Carrito.Select(x => new VentaDetalleDto
                {
                    Codigo = x.Codigo,
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.Precio,
                    Total = x.Total
                }).ToList()
            };

            var result = await _ventaApi.CrearVentaAsync(dto);

            if (!result.Success)
            {
                _notify.Error(result.Message);
                return result;
            }

            _notify.Success("Venta realizada correctamente");
            Limpiar();

            return result;
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
            return null;
        }
    }
    [RelayCommand]
    public async Task NuevoClienteAsync()
    {
        var vm = App.AppServices.GetRequiredService<ClienteOverlayViewModel>();

        var overlay = new ClienteOverlay(vm);
        _overlayService.Show(overlay);
    }
}
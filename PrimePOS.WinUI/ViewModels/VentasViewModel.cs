using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        AppSesionViewModel sesion)
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
        var res = await _turnoApi.ObtenerTurnoActivoAsync(_sesion.CajaId);

        if (res.Success && res.Data != null)
        {
            _sesion.TurnoActual = res.Data;
        }

        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning("Debe abrir un turno");
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
        catch (Exception)
        {
            _notify.Error("Error al buscar productos");
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
        var vm = new AbrirTurnoViewModel(
            _cajaApi,
            _turnoApi,
            _notify,
            _sesion
        );

        await vm.InicializarAsync();

        vm.OnCerrar += () => CerrarOverlay?.Invoke();

        MostrarOverlay?.Invoke(vm);
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

        vm.Cancelado += () => CerrarOverlay?.Invoke();

        vm.Confirmado += async (cobro) =>
        {
            await FacturarDesdeCobroAsync(cobro.Efectivo, cobro.Cambio);
            CerrarOverlay?.Invoke();
        };

        MostrarOverlay?.Invoke(vm);
    }

    // =========================
    // 🧾 FACTURAR DESDE COBRO
    // =========================

    public async Task FacturarDesdeCobroAsync(decimal efectivo, decimal cambio)
    {
        try
        {
            if (MetodoPagoSeleccionado == null)
            {
                _notify.Warning("Seleccione un método de pago");
                return;
            }

            var dto = new CrearVentaDto
            {
                //ClienteId = ClienteSeleccionado?.ClienteId,
                //ClienteNombre = ClienteSeleccionado?.Nombre,
                MetodoPagoId = MetodoPagoSeleccionado.MetodoPagoId,
                TurnoId = _sesion.TurnoActual!.TurnoId,

                Subtotal = Subtotal,
                Impuesto = Impuesto,
                Descuento = DescuentoMonto,
                Total = Total,
                Efectivo = efectivo,
                Cambio = cambio,

                Items = Carrito.Select(x => new VentaDetalleDto
                {
                    Codigo = x.Codigo,
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.Precio,
                    Total = x.Total
                }).ToList()
            };

            var resVenta = await _ventaApi.CrearVentaAsync(dto);

            if (!resVenta.Success)
            {
                _notify.Error(resVenta.Message);
                return;
            }

            var resFactura = await _facturaApi.GenerarFacturaAsync(resVenta.Data);

            if (!resFactura.Success)
            {
                _notify.Error(resFactura.Message);
                return;
            }

            //if (!string.IsNullOrEmpty(resFactura.Data.UrlPdf))
            //{
            //    Process.Start(new ProcessStartInfo
            //    {
            //        FileName = resFactura.Data.UrlPdf,
            //        UseShellExecute = true
            //    });
            //}

            _notify.Success("Venta realizada correctamente");

            Limpiar();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            _notify.Error("Error al procesar la venta");
        }
    }
}
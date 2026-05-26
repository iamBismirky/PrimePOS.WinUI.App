using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.ViewModels.Pages;
using PrimePOS.WinUI.Views.Overlays;
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
    private readonly TurnoApiService _turnoApi;
    private readonly VentaApiService _ventaApi;
    private readonly FacturaApiService _apiFactura;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;
    private readonly OverlayService _overlayService;
    private readonly PdfViewService _pdfService;
    public AppSesionViewModel AppSesion => _sesion;

    public VentaViewModel(ProductoApiService productoApi,
                          ClienteApiService clienteApi,
                          TurnoApiService turnoApi,
                          VentaApiService ventaApi,
                          FacturaApiService facturaApi,
                          NotificationService notify,
                          AppSesionViewModel sesion,
                          OverlayService overlay,
                          PdfViewService pdfService)
    {
        _productoApi = productoApi;
        _clienteApi = clienteApi;
        _turnoApi = turnoApi;
        _ventaApi = ventaApi;
        _apiFactura = facturaApi;
        _notify = notify;
        _sesion = sesion;
        _overlayService = overlay;
        _pdfService = pdfService;
    }


    [ObservableProperty] private string textoProducto = "";
    [ObservableProperty] private string textoCliente = "";

    [ObservableProperty] private ObservableCollection<ProductoDto> productos = [];
    [ObservableProperty] private ObservableCollection<ClienteDto> clientes = [];


    [ObservableProperty] private ClienteDto? clienteSeleccionado;

    [ObservableProperty] private ObservableCollection<CarritoViewModel> carrito = [];

    [ObservableProperty] public bool isLoading;

    private ClienteDto? _consumidorFinal;
    public decimal Subtotal => Carrito.Sum(x => x.Subtotal);
    public decimal Impuesto => Carrito.Sum(x => x.Itbis);
    public decimal Total => Subtotal + Impuesto;

    public int? TipoPrecioId { get; set; }


    private void NotificarTotales()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Impuesto));
        OnPropertyChanged(nameof(Total));
    }



    public async Task InicializarAsync()
    {

        await CargarConsumidorFinalAsync();
        await VerificarTurnoAsync();
    }



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

    private async Task CargarConsumidorFinalAsync()
    {



        var res = await _clienteApi.ObtenerClientePorIdAsync(1);

        if (res.Success && res.Data != null)
        {
            _consumidorFinal = res.Data;
            ClienteSeleccionado = res.Data;
        }

    }



    [RelayCommand]
    public async Task BuscarProductosAsync()
    {
        try
        {

            if (string.IsNullOrWhiteSpace(TextoProducto) || TextoProducto.Length < 2)
            {
                Productos.Clear();
                return;
            }

            var res = await _productoApi.BuscarProductosAsync(TextoProducto);


            if (!res.Success)
            {
                _notify.Warning(res.Message);

                return;
            }


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

    public async Task AgregarProductoCarritoAsync(ProductoDto dto)
    {
        var existente =
            Carrito.FirstOrDefault(p =>
                p.ProductoId == dto.ProductoId);

        if (dto.Existencia <= 0)
        {
            _notify.Warning($"No hay stock disponible de {dto.Nombre}");
            return;
        }

        if (dto.Existencia <= 5)
        {
            _notify.Warning($"Inventario bajo de {dto.Nombre} - Existen {dto.Existencia}");
        }

        if (!dto.Estado)
        {
            _notify.Warning($"El producto {dto.Nombre} está inactivo y no se puede vender");
            return;
        }

        decimal precio = ObtenerPrecio(dto);
        decimal itbisUnitario = ObtenerItbis(dto);

        if (existente != null)
        {
            existente.Cantidad++;
        }
        else
        {
            Carrito.Add(new CarritoViewModel
            {
                Codigo = dto.Codigo,
                ProductoId = dto.ProductoId,
                Nombre = dto.Nombre,
                Cantidad = 1,

                PrecioMinoristaBase = dto.PrecioMinorista,
                PrecioMayoristaBase = dto.PrecioMayorista,

                Precio = ClienteSeleccionado?.TipoPrecioId == 2
                                ? dto.PrecioMayorista
                                : dto.PrecioMinorista,

                ItbisUnitario = dto.ItbisMinorista
            });
        }

        NotificarTotales();
    }
    private decimal ObtenerPrecio(ProductoDto dto)
    {
        return ClienteSeleccionado?.TipoPrecioId == 2
            ? dto.PrecioMayorista
            : dto.PrecioMinorista;
    }
    private decimal ObtenerItbis(ProductoDto dto)
    {
        return ClienteSeleccionado?.TipoPrecioId == 2
            ? dto.ItbisMayorista
            : dto.ItbisMinorista;
    }
    [RelayCommand]
    public void EliminarProducto(CarritoViewModel item)
    {
        if (item == null) return;

        Carrito.Remove(item);
        NotificarTotales();
    }
    private void AplicarTipoPrecioAlCarrito()
    {
        if (ClienteSeleccionado == null)
            return;

        foreach (var item in Carrito)
        {
            item.Precio = ClienteSeleccionado.TipoPrecioId == 2
                ? item.PrecioMayoristaBase
                : item.PrecioMinoristaBase;

            item.NotificarTotales();
        }

        NotificarTotales();
    }

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

        AplicarTipoPrecioAlCarrito();
        _notify.Warning($"Tipo de venta {ClienteSeleccionado.Tipo}");
    }
    [RelayCommand]
    public async Task NuevoClienteAsync()
    {
        var vm = App.Services.GetRequiredService<ClienteOverlayViewModel>();
        await vm.InicializeAsync(null);
        var overlay = new ClienteOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;
    }






    [RelayCommand]
    private async Task LimpiarAsync()
    {
        Carrito.Clear();
        NotificarTotales();
        ClienteSeleccionado = _consumidorFinal;
        TextoProducto = "";
        TextoCliente = "";
    }



    [RelayCommand]
    private async Task AbrirTurnoAsync()
    {
        if (_sesion.HayTurnoAbierto)
        {
            _notify.Warning("Ya hay un turno abierto");
            return;
        }

        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning(
                "Debe abrir un turno para continuar");
        }

        var vm = App.Services.GetRequiredService<AbrirTurnoOverlayViewModel>();
        await vm.InicializarAsync();
        var overlay = new AbrirTurnoOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;

        await CargarConsumidorFinalAsync();

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

        var dto = new CobroVentaDto
        {
            Cliente = ClienteSeleccionado,
            Subtotal = Subtotal,
            Impuesto = Impuesto,
            Total = Total,
            Items = Carrito.Select(x => new CarritoItemDto
            {
                ProductoId = x.ProductoId,
                Nombre = x.Nombre,
                Codigo = x.Codigo,
                Cantidad = x.Cantidad,
                Precio = x.Precio,
                ItbisUnitario = x.ItbisUnitario,

            })
        .ToList()
        };
        var vm = App.Services.GetRequiredService<CobrarOverlayViewModel>();
        await vm.InicializarAsync(dto);
        var overlay = new CobrarOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;

        if (result)
        {
            Carrito.Clear();
            ClienteSeleccionado = _consumidorFinal;
            NotificarTotales();

        }



    }
    [RelayCommand]
    public async Task CerrarTurnoAsync()
    {
        if (!_sesion.HayTurnoAbierto)
        {
            _notify.Warning(
                "Debe abrir un turno para continuar");
            return;
        }
        var vm = App.Services.GetRequiredService<CerrarTurnoOverlayViewModel>();
        await vm.InicializarAsync();
        var overlay = new CerrarTurnoOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;
    }







}
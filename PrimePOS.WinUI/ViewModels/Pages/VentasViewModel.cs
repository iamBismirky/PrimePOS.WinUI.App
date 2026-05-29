using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly ProductoApiService _apiProducto;
    private readonly ClienteApiService _apiCliente;
    private readonly TurnoApiService _apiTurno;
    private readonly VentaApiService _apiVenta;
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
        _apiProducto = productoApi;
        _apiCliente = clienteApi;
        _apiTurno = turnoApi;
        _apiVenta = ventaApi;
        _apiFactura = facturaApi;
        _notify = notify;
        _sesion = sesion;
        _overlayService = overlay;
        _pdfService = pdfService;
    }



    private ClienteVentaDto? _consumidorFinal;


    [ObservableProperty]
    private string textoProducto = "";

    [ObservableProperty]
    private string textoCliente = "";

    [ObservableProperty]
    private ObservableCollection<ProductoVentaDto> productos = new();

    [ObservableProperty]
    private ObservableCollection<ClienteVentaDto> clientes = new();


    [ObservableProperty]
    private ClienteVentaDto? clienteSeleccionado;

    [ObservableProperty]
    private ObservableCollection<CarritoViewModel> carrito = new();

    [ObservableProperty]
    public bool isLoading;

    [ObservableProperty]
    private decimal subtotal;

    [ObservableProperty]
    private decimal impuesto;

    [ObservableProperty]
    private decimal total;





    public async Task InicializarAsync()
    {

        await CargarConsumidorFinalAsync();
        await VerificarTurnoAsync();
    }
    private void NotificarTotales()
    {
        Subtotal = Carrito.Sum(x => x.Subtotal);

        Impuesto = Carrito.Sum(x => x.Itbis);

        Total = Subtotal + Impuesto;
    }



    private async Task VerificarTurnoAsync()
    {
        var res = await _apiTurno.ObtenerTurnoActivoAsync();

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



        var res = await _apiVenta.CargarConsumidorFinalAsync();

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

            var res = await _apiVenta.BuscarProductosAsync(TextoProducto, ClienteSeleccionado?.TipoPrecioId ?? 1);


            if (!res.Success)
            {
                _notify.Warning(res.Message);

                return;
            }


            Productos = new ObservableCollection<ProductoVentaDto>(
                res.Data ?? new List<ProductoVentaDto>()
            );
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
    }

    [RelayCommand]
    public async Task SeleccionarProductoAsync(ProductoVentaDto producto)
    {
        if (producto == null) return;

        await AgregarProductoCarritoAsync(producto);

        TextoProducto = "";
        Productos.Clear();
    }

    public async Task AgregarProductoCarritoAsync(
    ProductoVentaDto dto)
    {
        var existente = Carrito.FirstOrDefault(
            p => p.ProductoId == dto.ProductoId);

        if (dto.Existencia <= 0)
        {
            _notify.Warning(
                $"No hay stock disponible de {dto.Nombre}");

            return;
        }

        if (dto.Existencia <= 5)
        {
            _notify.Warning(
                $"Inventario bajo de {dto.Nombre} - Existen {dto.Existencia}");
        }

        if (!dto.Estado)
        {
            _notify.Warning(
                $"El producto {dto.Nombre} está inactivo");

            return;
        }

        if (existente != null)
        {
            existente.Cantidad++;
        }
        else
        {
            Carrito.Add(new CarritoViewModel
            {
                ProductoId = dto.ProductoId,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Cantidad = 1,

                Precio = dto.Precio,

                ItbisUnitario = dto.ItbisUnitario
            });
        }

        NotificarTotales();
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

    }

    [RelayCommand]
    public async Task BuscarClientesAsync()
    {
        if (TextoCliente.Length < 2)
        {
            Clientes.Clear();
            return;
        }

        var res = await _apiVenta.BuscarClientesAsync(TextoCliente);

        if (res.Success && res.Data != null)
            Clientes = new ObservableCollection<ClienteVentaDto>(res.Data);
    }

    [RelayCommand]
    public void SeleccionarCliente(ClienteVentaDto cliente)
    {
        ClienteSeleccionado = cliente;
        TextoCliente = "";

        AplicarTipoPrecioAlCarrito();
        _notify.Info($"Tipo de venta {ClienteSeleccionado.TipoNombre}");
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
        TextoProducto = "";

        TextoCliente = "";

        ClienteSeleccionado = _consumidorFinal;
        Carrito.Clear();

        NotificarTotales();
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

            ClienteSeleccionado = _consumidorFinal;

            Carrito.Clear();
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
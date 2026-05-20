using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
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
        _turnoApi = turnoApi;
        _ventaApi = ventaApi;
        _facturaApi = facturaApi;
        _notify = notify;
        _sesion = sesion;
        _overlayService = overlay;
    }


    [ObservableProperty] private string textoProducto = "";
    [ObservableProperty] private string textoCliente = "";

    [ObservableProperty] private ObservableCollection<ProductoDto> productos = new();
    [ObservableProperty] private ObservableCollection<ClienteDto> clientes = new();


    [ObservableProperty] private ClienteDto? clienteSeleccionado;

    [ObservableProperty] private ObservableCollection<CarritoViewModel> carrito = new();

    [ObservableProperty] public bool isLoading;

    private ClienteDto? _consumidorFinal;

    public decimal Subtotal => Carrito.Sum(x => x.Subtotal);
    public decimal Impuesto => Carrito.Sum(x => x.Itbis);
    public decimal Total => Subtotal + Impuesto;



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
            Carrito.Add(new CarritoViewModel
            {
                Codigo = dto.Codigo,
                ProductoId = dto.ProductoId,
                Nombre = dto.Nombre,
                Precio = dto.PrecioVenta,
                ItbisUnitario = dto.Itbis,
                Cantidad = 1
            });

        NotificarTotales();
    }

    [RelayCommand]
    public void EliminarProducto(CarritoViewModel item)
    {
        if (item == null) return;

        Carrito.Remove(item);
        NotificarTotales();
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

        var vm = App.Services.GetRequiredService<AbrirTurnoViewModel>();
        await vm.InicializarAsync();
        var overlay = new AbrirTurnoOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;


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

        var dto = new CrearVentaDto
        {
            ClienteId = ClienteSeleccionado!.ClienteId,
            ClienteNombre = ClienteSeleccionado.Nombre,
            TurnoId = _sesion.TurnoActual!.TurnoId,
            Subtotal = Subtotal,
            Impuesto = Impuesto,
            Total = Total,
            Items = Carrito.Select(x => new VentaDetalleDto
            {
                Codigo = x.Codigo,
                ProductoId = x.ProductoId,
                Cantidad = x.Cantidad,
                PrecioUnitario = x.Precio,
                Total = x.Total
            }).ToList()
        };
        var vm = App.Services.GetRequiredService<CobrarViewModel>();
        await vm.InicializarAsync(dto);
        var overlay = new CobrarOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;

        if (result)
        {
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
        var vm = App.Services.GetRequiredService<CerrarTurnoViewModel>();
        await vm.InicializarAsync();
        var overlay = new CerrarTurnoOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;
    }




    [RelayCommand]
    public async Task NuevoClienteAsync()
    {
        var vm = new ClienteOverlayViewModel(
         _clienteApi,
         _notify);

        var overlay = new ClienteOverlay(vm);
        var result = await _overlayService.ShowAsync(overlay, vm);
        if (!result)
            return;
    }
}
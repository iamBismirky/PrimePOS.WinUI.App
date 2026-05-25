using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CobrarOverlayViewModel : ObservableObject, IOverlayViewModel
{


    private readonly VentaApiService _apiVenta;
    private readonly MetodoPagoApiService _apiMetodoPagoo;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;
    private readonly FacturaApiService _apiFactura;
    private readonly PdfViewService _pdfService;
    private readonly CatalogApiService _apiCatalog;


    private TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;

    public CobrarOverlayViewModel(VentaApiService apiVenta,
        MetodoPagoApiService apiMetodoPago,
        NotificationService notify,
        AppSesionViewModel sesion,
        FacturaApiService apiFactura,
        PdfViewService pdfService,
        CatalogApiService apiCatalog)
    {


        _apiVenta = apiVenta;
        _notify = notify;
        _apiMetodoPagoo = apiMetodoPago;
        _sesion = sesion;
        _apiFactura = apiFactura;
        _pdfService = pdfService;
        _apiCatalog = apiCatalog;
    }

    [ObservableProperty] private ObservableCollection<MetodoPagoDto> metodosPago = new();
    [ObservableProperty] private ObservableCollection<TipoVentaDto> tiposVenta = new();
    [ObservableProperty] private MetodoPagoDto? metodoPagoSeleccionado;
    [ObservableProperty] private TipoVentaDto? tipoVentaSeleccionado;
    [ObservableProperty] private ClienteDto? clienteSeleccionado;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private decimal total;
    [ObservableProperty] private decimal subtotal;
    [ObservableProperty] private decimal impuesto;
    [ObservableProperty] private decimal descuento;
    [ObservableProperty] private decimal descuentoSeleccionado;

    [ObservableProperty] private decimal montoRecibido;
    [ObservableProperty] private decimal totalOriginal;

    public CobroVentaDto CobroVenta { get; set; } = null!;
    public int VentaIdGenerada { get; private set; }


    public ObservableCollection<Decimal> Descuentos { get; } = new()
    { 0,5,10,15,20,25,30};


    public decimal Cambio => MontoRecibido > Total ? MontoRecibido - Total : 0;
    public decimal Faltante => MontoRecibido < Total ? Total - MontoRecibido : 0;
    public bool TieneDescuento => Descuento > 0;

    partial void OnMontoRecibidoChanged(decimal value)
    {
        OnPropertyChanged(nameof(Cambio));
        OnPropertyChanged(nameof(Faltante));
    }
    partial void OnDescuentoSeleccionadoChanged(decimal value)
    {
        RecalcularTotales();
    }
    private void RecalcularTotales()
    {
        Descuento =
            TotalOriginal * (DescuentoSeleccionado / 100);

        Total =
            TotalOriginal - Descuento;

        OnPropertyChanged(nameof(Cambio));
        OnPropertyChanged(nameof(Faltante));
        OnPropertyChanged(nameof(TieneDescuento));
    }
    public async Task InicializarAsync(CobroVentaDto cobro)
    {

        CobroVenta = cobro;
        ClienteSeleccionado = cobro.Cliente;
        Subtotal = cobro.Subtotal;
        Impuesto = cobro.Impuesto;
        TotalOriginal = cobro.Subtotal + cobro.Impuesto;
        Total = TotalOriginal;
        await CargarMetodosPagoAsync();
        await CargarTipoVentasAsync();
    }

    [RelayCommand]
    private async Task ConfirmarAsync()
    {

        try
        {
            IsLoading = true;

            ValidarAntesDeEnviar();

            var dto = BuildCrearVentaDto();

            Debug.WriteLine("ANTES API");
            var result = await _apiVenta.CrearVentaAsync(dto);
            Debug.WriteLine("DESPUÉS API");
            if (!result.Success)
            {
                _notify.Error(result.Message);
                return;
            }

            _notify.Success(result.Message);

            VentaIdGenerada = result.Data;

            Close(true);





        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }

    }

    private CrearVentaDto BuildCrearVentaDto()
    {
        return new CrearVentaDto
        {
            ClienteId = CobroVenta.Cliente!.ClienteId,
            ClienteNombre = CobroVenta.Cliente.Nombre,

            MetodoPagoId = MetodoPagoSeleccionado!.MetodoPagoId,

            TipoVentaId = TipoVentaSeleccionado!.TipoVentaId,

            TipoPrecioId = ClienteSeleccionado?.TipoPrecioId ?? 1,

            TurnoId = _sesion.TurnoActual!.TurnoId,
            CajaId = _sesion.TurnoActual!.CajaId,
            NumeroTurno = _sesion.TurnoActual.NumeroTurno,

            MontoRecibido = MontoRecibido,
            Descuento = Descuento,

            Items = CobroVenta.Items.Select(x => new VentaDetalleDto
            {
                ProductoId = x.ProductoId,
                ProductoNombre = x.Nombre,
                Codigo = x.Codigo,
                Cantidad = x.Cantidad,
                PrecioUnitario = x.Precio
            }).ToList()
        };
    }
    private void ValidarAntesDeEnviar()
    {
        if (MetodoPagoSeleccionado == null)
            throw new Exception("Seleccione método de pago");

        if (TipoVentaSeleccionado == null)
            throw new Exception("Seleccione tipo de venta");

        if (CobroVenta.Cliente == null && TipoVentaSeleccionado.TipoVentaId == 2)
            throw new Exception("Cliente requerido para crédito");

        if (MontoRecibido < Total && TipoVentaSeleccionado.TipoVentaId == 1)
            throw new Exception("Monto insuficiente");
    }
    private async Task CargarMetodosPagoAsync()
    {
        var res = await _apiMetodoPagoo.ObtenerMetodosPagoAsync();

        if (!res.Success || res.Data == null)
        {
            _notify.Warning(res.Message);
            return;
        }

        MetodosPago = new ObservableCollection<MetodoPagoDto>(res.Data);
        MetodoPagoSeleccionado = MetodosPago.FirstOrDefault();
    }
    private async Task CargarTipoVentasAsync()
    {
        var res = await _apiCatalog.ObtenerTodosTipoVentasAsync();

        if (!res.Success || res.Data == null)
        {
            _notify.Warning(res.Message);
            return;
        }

        TiposVenta = new ObservableCollection<TipoVentaDto>(res.Data);
        TipoVentaSeleccionado = TiposVenta.FirstOrDefault();
    }
    public void Close(bool result)
    {
        _tcs.TrySetResult(result);
    }
    [RelayCommand]
    private void Cancelar()
    {
        Close(false);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.ENTITIES.Enums;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CobrarViewModel : ObservableObject, IOverlayViewModel
{


    private readonly VentaApiService _apiVenta;
    private readonly MetodoPagoApiService _apiMetodoPagoo;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;
    private readonly FacturaApiService _apiFactura;
    private readonly PdfViewService _pdfService;

    private readonly TipoVenta _tipoVenta;
    private TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;

    public CobrarViewModel(VentaApiService apiVenta,
        MetodoPagoApiService apiMetodoPago,
        NotificationService notify,
        AppSesionViewModel sesion,
        FacturaApiService apiFactura,
        PdfViewService pdfService)
    {


        _apiVenta = apiVenta;
        _notify = notify;
        _apiMetodoPagoo = apiMetodoPago;
        _sesion = sesion;
        _apiFactura = apiFactura;
        _pdfService = pdfService;

    }

    [ObservableProperty] private ObservableCollection<MetodoPagoDto> metodosPago = new();
    [ObservableProperty] private MetodoPagoDto? metodoPagoSeleccionado;
    [ObservableProperty] private ClienteDto? clienteSeleccionado;
    [ObservableProperty] private TipoVenta tipoVenta = TipoVenta.Contado;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private decimal total;
    [ObservableProperty] private decimal subtotal;
    [ObservableProperty] private decimal impuesto;
    [ObservableProperty] private decimal descuento;
    [ObservableProperty] private decimal descuentoSeleccionado;
    public CrearVentaDto Venta { get; private set; } = null!;
    [ObservableProperty]
    private decimal totalOriginal;
    public IEnumerable<TipoVenta> TiposVenta => Enum.GetValues(typeof(TipoVenta)).Cast<TipoVenta>();

    public ObservableCollection<Decimal> Descuentos { get; } = new()
    { 0,5,10,15,20,25,30};

    [ObservableProperty]
    private decimal montoRecibido;


    public decimal Cambio => MontoRecibido > Venta.Total ? MontoRecibido - Total : 0;
    public decimal Faltante => MontoRecibido < Venta.Total ? Total - MontoRecibido : 0;
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
    public async Task InicializarAsync(CrearVentaDto venta)
    {
        _tcs = new TaskCompletionSource<bool>();
        Venta = venta;
        Subtotal = venta.Subtotal;
        Impuesto = venta.Impuesto;
        TotalOriginal = venta.Subtotal + venta.Impuesto;
        Total = TotalOriginal;
        await CargarMetodosPagoAsync();
    }

    [RelayCommand]
    private async Task ConfirmarAsync()
    {
        if (Venta.MontoPagado < Venta.Total)
        {
            _notify.Warning("El efectivo ingresado es menor al total a pagar");
            return;
        }

        if (MetodoPagoSeleccionado == null)
        {
            _notify.Warning("Seleccione un método de pago");
            return;
        }
        if (Venta.ClienteId == 0)
        {
            _notify.Warning("Seleccione un cliente");
            return;
        }


        try
        {
            IsLoading = true;


            var dto = new CrearVentaDto
            {
                ClienteId = Venta.ClienteId,
                ClienteNombre = Venta.ClienteNombre,
                MetodoPagoId = MetodoPagoSeleccionado.MetodoPagoId,
                TurnoId = _sesion.TurnoActual!.TurnoId,
                NumeroTurno = _sesion.TurnoActual.NumeroTurno,
                TipoVenta = TipoVenta,
                Subtotal = Venta.Subtotal,
                Impuesto = Venta.Impuesto,
                Descuento = Venta.Descuento,
                Total = Venta.Total,
                MontoPagado = Venta.MontoPagado,

                Items = Venta.Items.Select(x => new VentaDetalleDto
                {
                    Codigo = x.Codigo,
                    ProductoId = x.ProductoId,
                    ProductoNombre = x.ProductoNombre,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.PrecioUnitario,
                    AplicaItbis = x.AplicaItbis,
                    Itbis = x.Itbis,
                    Total = x.Total
                }).ToList()
            };

            var result = await _apiVenta.CrearVentaAsync(dto);

            if (!result.Success)
            {
                _notify.Error(result.Message);
                return;
            }

            _notify.Success("Venta realizada correctamente");

            // MOSTRAR PDF
            if (!string.IsNullOrWhiteSpace(result.Data?.UrlPdf))
            {
                await _pdfService.MostrarFacturaAsync(result.Data.UrlPdf);
            }
            Close(true);


        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
            return;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Cancelar()
    {
        Close(false);
    }
    public void FormatearEfectivo()
    {
        //MontoRecibido = MoneyHelper.ToString(Venta.MontoPagado);
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
    public void Close(bool result)
    {
        _tcs.TrySetResult(result);
    }
}
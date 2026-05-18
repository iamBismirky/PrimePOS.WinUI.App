using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CobrarViewModel : ObservableObject
{


    private readonly VentaApiService _apiVenta;
    private readonly MetodoPagoApiService _apiMetodoPagoo;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;

    private TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;

    public CobrarViewModel(VentaApiService apiVenta,
        MetodoPagoApiService apiMetodoPago,
        NotificationService notify,
        AppSesionViewModel sesion)
    {


        _apiVenta = apiVenta;
        _notify = notify;
        _apiMetodoPagoo = apiMetodoPago;
        _sesion = sesion;


    }

    [ObservableProperty] private ObservableCollection<MetodoPagoDto> metodosPago = new();
    [ObservableProperty] private MetodoPagoDto? metodoPagoSeleccionado;
    [ObservableProperty] private ClienteDto? clienteSeleccionado;
    [ObservableProperty] private bool isLoading;
    public CrearVentaDto Venta { get; private set; } = null!;

    private string efectivoTexto = "";
    public string EfectivoTexto
    {
        get => efectivoTexto;
        set
        {
            if (SetProperty(ref efectivoTexto, value))
            {
                Venta.Efectivo = MoneyHelper.ToDecimal(value);
            }
        }
    }

    public decimal Cambio => Venta.Efectivo - Venta.Total;

    //partial void OnEfectivoChanged(decimal value)
    //{
    //    OnPropertyChanged(nameof(Cambio));
    //}
    public async Task InicializarAsync(CrearVentaDto venta)
    {
        _tcs = new TaskCompletionSource<bool>();
        Venta = venta;

        await CargarMetodosPagoAsync();
    }

    [RelayCommand]
    private async Task<ApiResponse<VentaResponseDto>?> ConfirmarAsync()
    {
        if (Venta.Efectivo < Venta.Total)
        {
            _notify.Warning("El efectivo ingresado es menor al total a pagar");
            return null;
        }

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


        try
        {
            IsLoading = true;


            var dto = new CrearVentaDto
            {
                ClienteId = ClienteSeleccionado.ClienteId,
                ClienteNombre = ClienteSeleccionado.Nombre,
                MetodoPagoId = MetodoPagoSeleccionado.MetodoPagoId,
                TurnoId = _sesion.TurnoActual!.TurnoId,

                Subtotal = Venta.Subtotal,
                Impuesto = Venta.Impuesto,
                Descuento = Venta.Descuento,
                Total = Venta.Total,
                Efectivo = Venta.Efectivo,

                Items = Venta.Items.Select(x => new VentaDetalleDto
                {
                    Codigo = x.Codigo,
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.PrecioUnitario,
                    Total = x.Total
                }).ToList()
            };

            var result = await _apiVenta.CrearVentaAsync(dto);

            if (!result.Success)
            {
                _notify.Error(result.Message);
                return result;
            }

            _notify.Success("Venta realizada correctamente");
            _tcs.TrySetResult(true);

            return result;
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
            return null;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Cancelar()
    {
        _tcs.TrySetResult(false);
    }
    public void FormatearEfectivo()
    {
        EfectivoTexto = MoneyHelper.ToString(Venta.Efectivo);
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
}
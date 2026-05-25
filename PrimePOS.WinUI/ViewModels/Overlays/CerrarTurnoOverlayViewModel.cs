using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

public partial class CerrarTurnoOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly TurnoApiService _turnoApi;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;
    private readonly TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;
    public CerrarTurnoOverlayViewModel(
        TurnoApiService turnoApi,
        NotificationService notify,
        AppSesionViewModel sesion)
    {
        _turnoApi = turnoApi;
        _notify = notify;
        _sesion = sesion;
    }


    [ObservableProperty] private decimal montoInicial;
    [ObservableProperty] private decimal totalEfectivo;
    [ObservableProperty] private decimal totalTarjeta;
    [ObservableProperty] private decimal totalTransferencia;
    [ObservableProperty] private string? efectivoTexto;
    [ObservableProperty] private decimal efectivoContado;
    [ObservableProperty] private bool isLoading;

    public decimal Diferencia =>
        EfectivoContado - (MontoInicial + TotalEfectivo);

    partial void OnEfectivoTextoChanged(string? value)
    {
        EfectivoContado = MoneyHelper.ToDecimal(value);
        //OnPropertyChanged(nameof(Diferencia));
    }
    partial void OnEfectivoContadoChanged(decimal value)
    {
        OnPropertyChanged(nameof(Diferencia));

    }


    // 🔹 INIT
    public async Task InicializarAsync()
    {


        if (_sesion.TurnoActual == null)
            return;

        var turnoId = _sesion.TurnoActual.TurnoId;

        var res = await _turnoApi.ObtenerResumenAsync(turnoId);

        if (!res.Success || res.Data == null)
        {
            _notify.Error(res.Message);
            return;
        }

        MontoInicial = res.Data.MontoInicial;
        TotalEfectivo = res.Data.TotalEfectivo;
        TotalTarjeta = res.Data.TotalTarjeta;
        TotalTransferencia = res.Data.TotalTransferencia;
    }

    // 🔹 CERRAR
    [RelayCommand]
    private async Task CerrarAsync()
    {
        try
        {
            IsLoading = true;
            var dto = new CierreTurnoDto
            {
                TurnoId = _sesion.TurnoActual!.TurnoId,
                MontoInicial = MontoInicial,
                TotalEfectivo = TotalEfectivo,
                TotalTarjeta = TotalTarjeta,
                TotalTransferencia = TotalTransferencia,
                EfectivoContado = EfectivoContado
            };

            var res = await _turnoApi.CerrarTurnoAsync(dto);

            if (!res.Success)
            {
                _notify.Error(res.Message);
                return;
            }

            _sesion.CerrarTurno();

            _notify.Success("Turno cerrado correctamente");

            Close(true);
        }
        catch (Exception ex)
        {
            _notify.Error("Error al cerrar el turno: " + ex.Message);
        }
        finally
        {
            IsLoading = false;
        }

    }

    // 🔹 CANCELAR
    [RelayCommand]
    private void Cancelar()
    {
        Close(false);
    }
    public void Close(bool result = false)
    {
        _tcs.TrySetResult(result);
    }
    public void FormatearEfectivo()
    {
        EfectivoTexto = MoneyHelper.ToString(EfectivoContado);
    }
}
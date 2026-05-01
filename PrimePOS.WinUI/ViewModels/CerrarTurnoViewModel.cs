using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

public partial class CerrarTurnoViewModel : ObservableObject
{
    private readonly TurnoApiService _turnoApi;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;

    public CerrarTurnoViewModel(
        TurnoApiService turnoApi,
        NotificationService notify,
        AppSesionViewModel sesion)
    {
        _turnoApi = turnoApi;
        _notify = notify;
        _sesion = sesion;
    }

    // 🔹 DATOS
    [ObservableProperty] private decimal montoInicial;
    [ObservableProperty] private decimal totalEfectivo;
    [ObservableProperty] private decimal totalTarjeta;
    [ObservableProperty] private decimal totalTransferencia;
    [ObservableProperty] private decimal efectivoContado;

    public decimal Diferencia =>
        EfectivoContado - (MontoInicial + TotalEfectivo);

    partial void OnEfectivoContadoChanged(decimal value)
    {
        OnPropertyChanged(nameof(Diferencia));
    }

    public event Action? OnCerrar;

    // 🔹 INIT
    public async Task InicializarAsync()
    {
        var turnoId = _sesion.TurnoActual!.TurnoId;

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

        OnCerrar?.Invoke();
    }

    // 🔹 CANCELAR
    [RelayCommand]
    private void Cancelar()
    {
        OnCerrar?.Invoke();
    }
}
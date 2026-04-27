using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

public partial class AbrirTurnoViewModel : ObservableObject
{
    private readonly CajaApiService _cajaApi;
    private readonly TurnoApiService _turnoApi;
    private readonly NotificationService _notify;
    private readonly AppSesionViewModel _sesion;

    public AbrirTurnoViewModel(
        CajaApiService cajaApi,
        TurnoApiService turnoApi,
        NotificationService notify,
        AppSesionViewModel sesion)
    {
        _cajaApi = cajaApi;
        _turnoApi = turnoApi;
        _notify = notify;
        _sesion = sesion;
    }

    // 🔹 PROPIEDADES
    [ObservableProperty] private ObservableCollection<CajaDto> cajas = new();
    [ObservableProperty] private CajaDto? cajaSeleccionada;
    [ObservableProperty] private decimal montoInicial;
    [ObservableProperty] private string turnoTexto = "";

    public event Action? OnCerrar;

    // 🔹 INIT
    public async Task InicializarAsync()
    {
        var res = await _cajaApi.ObtenerCajasAsync();

        if (!res.Success || res.Data == null)
        {
            _notify.Error(res.Message);
            return;
        }

        Cajas = new ObservableCollection<CajaDto>(res.Data);
        CajaSeleccionada = Cajas.FirstOrDefault();

        var numero = await _turnoApi.ObtenerSiguienteTurnoAsync();
        TurnoTexto = $"Turno: {DateTime.Today:dd/MM/yyyy} - T{numero}";
    }

    // 🔹 ABRIR
    [RelayCommand]
    private async Task AbrirAsync()
    {
        if (CajaSeleccionada == null)
        {
            _notify.Warning("Seleccione una caja");
            return;
        }

        var dto = new CrearTurnoDto
        {
            CajaId = CajaSeleccionada.CajaId,
            MontoInicial = MontoInicial
        };

        var res = await _turnoApi.AbrirTurnoAsync(dto);

        if (!res.Success)
        {
            _notify.Error(res.Message);
            return;
        }

        _sesion.AbrirTurno(res.Data);

        _notify.Success("Turno abierto");

        OnCerrar?.Invoke();
    }

    // 🔹 CANCELAR
    [RelayCommand]
    private void Cancelar()
    {
        OnCerrar?.Invoke();
    }
}
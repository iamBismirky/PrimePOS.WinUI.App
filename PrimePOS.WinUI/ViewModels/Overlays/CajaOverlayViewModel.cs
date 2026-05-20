using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class CajaOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly CajaApiService _api;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public CajaOverlayViewModel(
        CajaApiService api,
        NotificationService notify,
        CajaDto? caja = null)
    {
        _api = api;
        _notify = notify;

        if (caja != null)
        {
            Caja = caja;
            Nombre = caja.Nombre;
            Estado = caja.Estado;
        }
    }

    [ObservableProperty]
    private CajaDto? caja;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private bool estado = true;

    [ObservableProperty]
    private bool isLoading;


    public async Task InicializarAsync()
    {

    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                _notify.Warning(
                    "El nombre es obligatorio");

                return;
            }

            if (Caja == null)
            {
                var res = await _api.CrearCajaAsync(
                    new CajaDto
                    {
                        Nombre = Nombre.Trim(),
                        Estado = Estado
                    });

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al crear caja");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Caja creada");
            }
            else
            {
                Caja.Nombre = Nombre.Trim();
                Caja.Estado = Estado;

                var res = await _api.ActualizarCajaAsync(
                    Caja.CajaId,
                    Caja);

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al actualizar caja");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Caja actualizada");
            }

            Limpiar();

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

    [RelayCommand]
    private void Cancelar()
    {
        Limpiar();

        Close(false);
    }
    public void Close(bool result)
    {
        Limpiar();
        _tcs.TrySetResult(result);
    }
    private void Limpiar()
    {
        Caja = null;

        Nombre = "";

        Estado = true;
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class RolOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly RolApiService _api;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public RolOverlayViewModel(
        RolApiService api,
        NotificationService notify,
        RolDto? rol = null)
    {
        _api = api;
        _notify = notify;

        if (rol != null)
        {
            Rol = rol;

            Nombre = rol.Nombre;
            Estado = rol.Estado;
        }
    }

    [ObservableProperty]
    private RolDto? rol;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private bool estado = true;

    [ObservableProperty]
    private bool isLoading = false;


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

            if (Rol == null)
            {
                var result =
                    await _api.CrearRolAsync(
                        new RolDto
                        {
                            Nombre = Nombre.Trim(),
                            Estado = Estado
                        });

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al crear rol");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Rol creado");
            }
            else
            {
                Rol.Nombre = Nombre.Trim();
                Rol.Estado = Estado;

                var result =
                    await _api.ActualizarRolAsync(
                        Rol.RolId,
                        Rol);

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al actualizar rol");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Rol actualizado");
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
    public void Close(bool result = false)
    {
        _tcs.TrySetResult(result);
    }
    private void Limpiar()
    {
        Rol = null;

        Nombre = "";

        Estado = true;
    }
}
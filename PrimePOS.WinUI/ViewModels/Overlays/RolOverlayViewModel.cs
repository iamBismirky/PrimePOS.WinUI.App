using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class RolOverlayViewModel : ObservableObject
{
    private readonly RolApiService _api;
    private readonly NotificationService _notify;

    private TaskCompletionSource<bool> _tcs = new();

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


    public async Task InicializarAsync()
    {
        _tcs = new TaskCompletionSource<bool>();
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

            _tcs.TrySetResult(true);
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

        _tcs.TrySetResult(false);
    }

    private void Limpiar()
    {
        Rol = null;

        Nombre = "";

        Estado = true;
    }
}
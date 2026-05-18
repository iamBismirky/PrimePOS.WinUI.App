using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class CategoriaOverlayViewModel : ObservableObject
{
    private readonly CategoriaApiService _api;
    private readonly NotificationService _notify;

    private TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public CategoriaOverlayViewModel(
        CategoriaApiService api,
        NotificationService notify,
        CategoriaDto? categoria = null)
    {
        _api = api;
        _notify = notify;

        if (categoria != null)
        {
            Categoria = categoria;

            Nombre = categoria.Nombre ?? "";
            Estado = categoria.Estado;
        }
    }

    [ObservableProperty]
    private CategoriaDto? categoria;

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

            if (Categoria == null)
            {
                var result =
                    await _api.CrearCategoriaAsync(
                        new CategoriaDto
                        {
                            Nombre = Nombre.Trim(),
                            Estado = Estado
                        });

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al crear categoría");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Categoría creada");
            }
            else
            {
                Categoria.Nombre = Nombre.Trim();
                Categoria.Estado = Estado;

                var result =
                    await _api.ActualizarCategoriaAsync(
                        Categoria.CategoriaId,
                        Categoria);

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al actualizar categoría");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Categoría actualizada");
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
        Categoria = null;

        Nombre = "";

        Estado = true;
    }
}
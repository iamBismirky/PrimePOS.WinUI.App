using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class RolViewModel : ObservableObject
{
    private readonly RolApiService _api;
    private readonly NotificationService _notify;

    public RolViewModel(RolApiService api, NotificationService notify)
    {
        _api = api;
        _notify = notify;
    }

    // =========================
    // PROPIEDADES
    // =========================

    [ObservableProperty]
    private ObservableCollection<RolDto> roles = new();

    [ObservableProperty]
    private RolDto? rolSeleccionado;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private bool estado = true;

    [ObservableProperty]
    private string buscar = "";

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isOverlayVisible;

    // cache para filtros
    private List<RolDto> _cache = new();

    // =========================
    // VISIBILIDAD (SIN CONVERTER)
    // =========================
    public Visibility OverlayVisibility =>
        IsOverlayVisible ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsOverlayVisibleChanged(bool value)
    {
        OnPropertyChanged(nameof(OverlayVisibility));
    }

    public Visibility LoadingVisibility =>
        IsLoading ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsLoadingChanged(bool value)
    {
        OnPropertyChanged(nameof(LoadingVisibility));
    }

    [RelayCommand]
    public async Task CargarAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _api.ObtenerRolesAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar cajas");
                return;
            }

            _cache = res.Data ?? new List<RolDto>();
            Roles = new ObservableCollection<RolDto>(res.Data ?? new());
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
    public async Task GuardarAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                _notify.Warning("El nombre es obligatorio");
                return;
            }


            if (RolSeleccionado == null)
            {


                var res = await _api.CrearRolAsync(new RolDto
                {
                    Nombre = Nombre.Trim(),
                    Estado = Estado
                });

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al crear caja");
                    return;
                }

                _notify.Success(res.Message ?? "Caja creada correctamente");
            }
            else
            {
                RolSeleccionado.Nombre = Nombre.Trim();
                RolSeleccionado.Estado = Estado;

                var res = await _api.ActualizarRolAsync(
                    RolSeleccionado.RolId,
                    RolSeleccionado);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar rol");
                    return;
                }

                _notify.Success(res.Message ?? "Rol actualizado correctamente");
            }

            await CargarAsync();
            Limpiar();
            CerrarOverlay();
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
    }

    [RelayCommand]
    public void Editar(RolDto rol)
    {
        RolSeleccionado = rol;
        Nombre = rol.Nombre;
        Estado = rol.Estado;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(RolDto rol)
    {
        var res = await _api.DesactivarRolAsync(rol.RolId);

        if (!res.Success)
        {
            _notify.Error(res.Message ?? "No se pudo desactivar");
            return;
        }

        _notify.Success(res.Message ?? "Caja desactivada");
        await CargarAsync();
    }

    [RelayCommand]
    public void Filtrar()
    {
        if (string.IsNullOrWhiteSpace(Buscar))
        {
            Roles = new ObservableCollection<RolDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Roles = new ObservableCollection<RolDto>(filtradas);
    }

    [RelayCommand]
    public void Nuevo()
    {
        Limpiar();
        AbrirOverlay();
    }

    [RelayCommand]
    public void Limpiar()
    {
        Nombre = "";
        RolSeleccionado = null;
    }

    [RelayCommand]
    public void AbrirOverlay()
    {
        IsOverlayVisible = true;
    }

    [RelayCommand]
    public void CerrarOverlay()
    {
        IsOverlayVisible = false;
        Limpiar();
    }
}
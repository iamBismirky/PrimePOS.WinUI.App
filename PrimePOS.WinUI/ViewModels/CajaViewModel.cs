using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CajaViewModel : ObservableObject
{
    private readonly CajaApiService _api;
    private readonly NotificationService _notify;

    public CajaViewModel(CajaApiService api, NotificationService notify)
    {
        _api = api;
        _notify = notify;
    }

    // =========================
    // PROPIEDADES
    // =========================

    [ObservableProperty]
    private ObservableCollection<CajaDto> cajas = new();

    [ObservableProperty]
    private CajaDto? cajaSeleccionada;

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
    private List<CajaDto> _cache = new();

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

            var res = await _api.ObtenerCajasAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar cajas");
                return;
            }

            _cache = res.Data ?? new List<CajaDto>();
            Cajas = new ObservableCollection<CajaDto>(res.Data ?? new());
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


            if (CajaSeleccionada == null)
            {


                var res = await _api.CrearCajaAsync(new CajaDto
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
                CajaSeleccionada.Nombre = Nombre.Trim();
                CajaSeleccionada.Estado = Estado;

                var res = await _api.ActualizarCajaAsync(
                    CajaSeleccionada.CajaId,
                    CajaSeleccionada);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar caja");
                    return;
                }

                _notify.Success(res.Message ?? "Caja actualizada correctamente");
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
    public void Editar(CajaDto caja)
    {
        CajaSeleccionada = caja;
        Nombre = caja.Nombre;
        Estado = caja.Estado;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(CajaDto caja)
    {
        var res = await _api.DesactivarCajaAsync(caja.CajaId);

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
            Cajas = new ObservableCollection<CajaDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Cajas = new ObservableCollection<CajaDto>(filtradas);
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
        CajaSeleccionada = null;
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CategoriaViewModel : ObservableObject
{
    private readonly CategoriaApiService _api;
    private readonly NotificationService _notify;

    public CategoriaViewModel(CategoriaApiService api, NotificationService notify)
    {
        _api = api;
        _notify = notify;
    }

    // =========================
    // PROPIEDADES
    // =========================

    [ObservableProperty]
    private ObservableCollection<CategoriaDto> categorias = new();

    [ObservableProperty]
    private CategoriaDto? categoriaSeleccionada;

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
    private List<CategoriaDto> _cache = new();

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

            var res = await _api.ObtenerCategoriasAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar categorias");
                return;
            }

            _cache = res.Data ?? new List<CategoriaDto>();
            Categorias = new ObservableCollection<CategoriaDto>(res.Data ?? new());
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


            if (CategoriaSeleccionada == null)
            {


                var res = await _api.CrearCategoriaAsync(new CategoriaDto
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
                CategoriaSeleccionada.Nombre = Nombre.Trim();
                CategoriaSeleccionada.Estado = Estado;

                var res = await _api.ActualizarCategoriaAsync(
                    CategoriaSeleccionada.CategoriaId,
                    CategoriaSeleccionada);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar categoria");
                    return;
                }

                _notify.Success(res.Message ?? "Categoria actualizada correctamente");
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
    public void Editar(CategoriaDto categoria)
    {
        CategoriaSeleccionada = categoria;
        Nombre = categoria.Nombre;
        Estado = categoria.Estado;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(CategoriaDto categoria)
    {
        var res = await _api.DesactivarCategoriaAsync(categoria.CategoriaId);

        if (!res.Success)
        {
            _notify.Error(res.Message ?? "No se pudo desactivar");
            return;
        }

        _notify.Success(res.Message ?? "Categoria desactivada");
        await CargarAsync();
    }

    [RelayCommand]
    public void Filtrar()
    {
        if (string.IsNullOrWhiteSpace(Buscar))
        {
            Categorias = new ObservableCollection<CategoriaDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Categorias = new ObservableCollection<CategoriaDto>(filtradas);
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
        Estado = true;
        CategoriaSeleccionada = null;
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
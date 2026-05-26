using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.Views.Overlays;
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
    private readonly OverlayService _overlayService;


    public CategoriaViewModel(CategoriaApiService api, NotificationService notify, OverlayService overlayService)
    {
        _api = api;
        _notify = notify;
        _overlayService = overlayService;
    }


    [ObservableProperty]
    private ObservableCollection<CategoriaDto> categorias = [];

    [ObservableProperty]
    private string buscar = "";

    [ObservableProperty]
    private bool isLoading;

    private List<CategoriaDto> _cache = [];


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

            _cache = res.Data ?? [];
            Categorias = new ObservableCollection<CategoriaDto>(res.Data ?? []);
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
    public async Task EditarAsync(CategoriaDto categoria)
    {
        var vm = new CategoriaOverlayViewModel(_api, _notify, categoria);

        var overlay = new CategoriaOverlay(vm);

        var actualizado = await _overlayService.ShowAsync(overlay, vm);
        if (!actualizado)
            return;
        if (actualizado)
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    public async Task DesactivarAsync(CategoriaDto categoria)
    {
        var confirmado =
            await _overlayService
                .ConfirmAsync(
                    "Desactivar categoría",
                    $"¿Desea desactivar {categoria.Nombre}?");

        if (!confirmado)
            return;

        var res =
            await _api.DesactivarCategoriaAsync(categoria.CategoriaId);

        if (!res.Success)
        {
            _notify.Error(
                res.Message ?? "No se pudo desactivar");

            return;
        }

        _notify.Success(
            res.Message ?? "Categoría desactivada");

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
    public async Task NuevoAsync()
    {
        var vm = App.Services.GetRequiredService<CategoriaOverlayViewModel>();
        var overlay = new CategoriaOverlay(vm);

        var creado = await _overlayService.ShowAsync(overlay, vm);
        if (!creado)
            return;

        if (creado)
        {
            await CargarAsync();
        }
    }


}
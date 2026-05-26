using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Caja;
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

public partial class CajaViewModel : ObservableObject
{
    private readonly CajaApiService _api;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;

    public CajaViewModel(CajaApiService api, NotificationService notify, OverlayService overlayService)
    {
        _api = api;
        _notify = notify;
        _overlayService = overlayService;
    }


    [ObservableProperty]
    private ObservableCollection<CajaDto> cajas = [];

    [ObservableProperty]
    private CajaDto? cajaSeleccionada;

    [ObservableProperty]
    private string buscar = "";

    [ObservableProperty]
    private bool isLoading;

    private List<CajaDto> _cache = [];



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

            _cache = res.Data ?? [];
            Cajas = new ObservableCollection<CajaDto>(res.Data ?? []);
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
    public async Task NuevoAsync()
    {
        var vm = App.Services.GetRequiredService<CajaOverlayViewModel>();

        var overlay = new CajaOverlay(vm);

        var creado = await _overlayService.ShowAsync(overlay, vm);
        if (!creado)
            return;
        if (creado)
        {
            await CargarAsync();
        }

    }

    [RelayCommand]
    public async Task EditarAsync(CajaDto caja)
    {


        if (caja == null)
        {
            _notify.Warning("Seleccione una caja");
            return;
        }

        var vm = new CajaOverlayViewModel(_api, _notify, caja)
        {
            Caja = caja
        };

        var overlay = new CajaOverlay(vm);

        var actualizado = await _overlayService.ShowAsync(overlay, vm);
        if (!actualizado)
            return;

        if (actualizado)
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    public async Task DesactivarAsync(CajaDto caja)
    {

        var confirmado = await _overlayService.ConfirmAsync(
            "¿Está seguro de desactivar esta caja?",
            $"¿Seguro que deseas eliminar '{caja.Nombre}'?");

        if (!confirmado)
            return;

        var res = await _api.DesactivarCajaAsync(caja.CajaId);

        if (!res.Success)
        {
            _notify.Error(res.Message ?? "No se pudo desactivar");
            return;
        }

        _notify.Success(res.Message ?? "Caja desactivada");
        await CargarAsync();
    }





}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Rol;
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

public partial class RolViewModel : ObservableObject
{
    private readonly RolApiService _api;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;

    public RolViewModel(RolApiService api, NotificationService notify, OverlayService overlayService)
    {
        _api = api;
        _notify = notify;
        _overlayService = overlayService;
    }


    [ObservableProperty]
    private ObservableCollection<RolDto> roles = new();

    [ObservableProperty]
    private RolDto? rolSeleccionado;


    [ObservableProperty]
    private string buscar = "";

    [ObservableProperty]
    private bool isLoading;

    private List<RolDto> _cache = new();



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
    public async Task EditarAsync(RolDto rol)
    {
        var vm = new RolOverlayViewModel(
            _api,
            _notify,
            rol);
        var overlay = new RolOverlay(vm);

        var actualizado = await _overlayService.ShowAsync(overlay, vm);
        if (!actualizado)
            return;

        if (actualizado)
        {
            await CargarAsync();
        }
    }


    [RelayCommand]
    public async Task DesactivarAsync(RolDto rol)
    {
        var confirmado = await _overlayService.ConfirmAsync(
            "Desactivar rol",
            $"¿Estás seguro de que deseas desactivar este rol {rol.Nombre}?");

        if (!confirmado)
            return;

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
    public async Task NuevoAsync()
    {
        var vm = App.Services.GetRequiredService<RolOverlayViewModel>();
        var overlay = new RolOverlay(vm);
        var creado = await _overlayService.ShowAsync(overlay, vm);
        if (!creado)
            return;
        if (creado)
        {
            await CargarAsync();
        }

    }


}
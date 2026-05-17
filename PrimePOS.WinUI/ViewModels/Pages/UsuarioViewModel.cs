using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.Contracts.DTOs.Usuario;
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

public partial class UsuarioViewModel : ObservableObject
{
    private readonly UsuarioApiService _apiUsuario;
    private readonly RolApiService _apiRol;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;

    public UsuarioViewModel(UsuarioApiService apiUsuario, RolApiService apiRol, NotificationService notify, OverlayService overlayService)
    {
        _apiUsuario = apiUsuario;
        _apiRol = apiRol;
        _notify = notify;
        _overlayService = overlayService;
    }



    [ObservableProperty] private ObservableCollection<UsuarioDto> usuarios = new();
    [ObservableProperty] private ObservableCollection<RolDto> roles = new();
    [ObservableProperty] private UsuarioDto? usuarioSeleccionado;
    [ObservableProperty] private RolDto? rolSeleccionado;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;

    private List<UsuarioDto> _cache = new();




    [RelayCommand]
    public async Task CargarUsuariosAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiUsuario.ObtenerUsuariosAsync();
            if (!res.Success)
            {
                _notify.Error(res.Message);
                return;
            }

            _cache = res.Data ?? new List<UsuarioDto>();
            Usuarios = new ObservableCollection<UsuarioDto>(res.Data ?? new());
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
    public async Task CargarRolesAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiRol.ObtenerRolesAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message);
                return;
            }


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
    public async Task EditarAsync(UsuarioDto usuario)
    {
        var vm = new UsuarioOverlayViewModel(_apiUsuario, _apiRol, _notify, usuario);
        await vm.InicializarAsync();
        var overlay = new UsuarioOverlay(vm);
        var actualizado = await _overlayService.ShowUsuarioAsync(overlay, vm);
        if (actualizado)
        {
            await CargarUsuariosAsync();
        }

    }

    [RelayCommand]
    public async Task DesactivarAsync(UsuarioDto usuario)
    {
        var confirm = await _overlayService.ConfirmAsync("Descativr Usuario",
            $"¿Está seguro de que desea desactivar este usuario {usuario.Nombre}?");

        if (!confirm)
            return;

        var res = await _apiUsuario.DesactivarUsuarioAsync(usuario.UsuarioId);

        if (!res.Success)
        {
            _notify.Error(res.Message);
            return;
        }

        _notify.Success(res.Message);
        await CargarUsuariosAsync();
    }

    [RelayCommand]
    public void Filtrar()
    {
        if (string.IsNullOrWhiteSpace(Buscar))
        {
            Usuarios = new ObservableCollection<UsuarioDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => (x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase)) ||
            (x.Codigo?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (x.Username?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (x.Apellidos?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false)

            ).ToList();

        Usuarios = new ObservableCollection<UsuarioDto>(filtradas);
    }

    [RelayCommand]
    public async Task NuevoAsync()
    {
        var vm = new UsuarioOverlayViewModel(_apiUsuario, _apiRol, _notify);
        await vm.InicializarAsync();
        var overlay = new UsuarioOverlay(vm);
        var creado = await _overlayService.ShowUsuarioAsync(overlay, vm);
        if (creado)
        {
            await CargarUsuariosAsync();
        }

    }

}
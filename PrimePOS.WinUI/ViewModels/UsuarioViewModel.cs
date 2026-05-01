using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
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

    public UsuarioViewModel(UsuarioApiService apiUsuario, RolApiService apiRol, NotificationService notify)
    {
        _apiUsuario = apiUsuario;
        _apiRol = apiRol;
        _notify = notify;
    }



    [ObservableProperty] private ObservableCollection<UsuarioDto> usuarios = new();
    [ObservableProperty] private ObservableCollection<RolDto> roles = new();
    [ObservableProperty] private UsuarioDto? usuarioSeleccionado;
    [ObservableProperty] private RolDto? rolSeleccionado;
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string apellidos = "";
    [ObservableProperty] private string username = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string codigo = "";
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isOverlayVisible;
    [ObservableProperty] private bool esEdicion;

    private List<UsuarioDto> _cache = new();

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
    public async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (UsuarioSeleccionado != null)
            {

                var dto = new ActualizarUsuarioDto
                {
                    UsuarioId = UsuarioSeleccionado!.UsuarioId,
                    Nombre = Nombre.Trim(),
                    Apellidos = Apellidos.Trim(),
                    Username = Username.Trim(),
                    RolId = RolSeleccionado!.RolId,
                    Estado = Estado,
                };

                var res = await _apiUsuario.ActualizarUsuarioAsync(UsuarioSeleccionado.UsuarioId, dto);
                if (!res.Success)
                {
                    _notify.Error(res.Message);
                    return;
                }

                _notify.Success(res.Message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Apellidos) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
                {
                    _notify.Warning("Todos los campos son obligatorios");
                    return;
                }
                if (RolSeleccionado == null)
                {
                    _notify.Warning("Debe seleccionar un rol");
                    return;
                }
                var dto = new CrearUsuarioDto
                {
                    Nombre = Nombre.Trim(),
                    Apellidos = Apellidos.Trim(),
                    Username = Username.Trim(),
                    Password = Password.Trim(),
                    RolId = RolSeleccionado!.RolId,
                    Estado = Estado,
                };

                var res = await _apiUsuario.CrearUsuarioAsync(dto);
                if (!res.Success)
                {
                    _notify.Error(res.Message);
                    return;
                }

                _notify.Success(res.Message);

            }


            await CargarUsuariosAsync();
            Limpiar();
            CerrarOverlay();
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
        EsEdicion = false;
        UsuarioSeleccionado = usuario;
        await CargarRolesAsync();
        Codigo = usuario.Codigo;
        Nombre = usuario.Nombre;
        Apellidos = usuario.Apellidos;
        Username = usuario.Username;
        RolSeleccionado = Roles.FirstOrDefault(r => r.RolId == usuario.RolId);
        Estado = usuario.Estado;
        FechaRegistro = usuario.FechaRegistro;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(UsuarioDto usuario)
    {
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
        try
        {
            IsLoading = true;
            Limpiar();
            await CargarRolesAsync();
            AbrirOverlay();
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    public void Limpiar()
    {
        Codigo = "";
        Nombre = "";
        Username = "";
        Password = "";
        Apellidos = "";
        RolSeleccionado = null;
        FechaRegistro = DateTime.Now;
        Estado = true;
        UsuarioSeleccionado = null;
        EsEdicion = false;
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
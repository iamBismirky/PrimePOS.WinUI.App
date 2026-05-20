using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class UsuarioOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly UsuarioApiService _apiUsuario;
    private readonly RolApiService _apiRol;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public UsuarioOverlayViewModel(
        UsuarioApiService apiUsuario,
        RolApiService apiRol,
        NotificationService notify,
        UsuarioDto? usuario = null)
    {
        _apiUsuario = apiUsuario;
        _apiRol = apiRol;
        _notify = notify;

        Usuario = usuario;
    }

    // =========================
    // PROPIEDADES
    // =========================

    [ObservableProperty]
    private UsuarioDto? usuario;

    [ObservableProperty]
    private ObservableCollection<RolDto> roles = [];

    [ObservableProperty]
    private RolDto? rolSeleccionado;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string apellidos = "";

    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private bool estado = true;

    [ObservableProperty]
    private DateTime fechaRegistro = DateTime.Today;

    [ObservableProperty]
    private bool isLoading;
    [ObservableProperty]
    private bool esEdicion = false;


    public async Task InicializarAsync()
    {

        await CargarRolesAsync();

        if (Usuario != null)
        {
            Codigo = Usuario.Codigo;

            Nombre = Usuario.Nombre;
            Apellidos = Usuario.Apellidos;

            Username = Usuario.Username;

            Estado = Usuario.Estado;

            FechaRegistro = Usuario.FechaRegistro;

            RolSeleccionado =
                Roles.FirstOrDefault(
                    x => x.RolId == Usuario.RolId);
        }
    }



    [RelayCommand]
    private async Task CargarRolesAsync()
    {
        try
        {
            IsLoading = true;

            var res =
                await _apiRol.ObtenerRolesAsync();

            if (!res.Success)
            {
                _notify.Error(
                    res.Message ?? "Error al cargar roles");

                return;
            }

            Roles =
                new ObservableCollection<RolDto>(
                    res.Data ?? []);
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

            if (string.IsNullOrWhiteSpace(Apellidos))
            {
                _notify.Warning(
                    "Los apellidos son obligatorios");

                return;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                _notify.Warning(
                    "El username es obligatorio");

                return;
            }

            if (RolSeleccionado == null)
            {
                _notify.Warning(
                    "Debe seleccionar un rol");

                return;
            }


            if (Usuario != null)
            {
                var dto =
                    new ActualizarUsuarioDto
                    {
                        UsuarioId = Usuario.UsuarioId,

                        Nombre = Nombre.Trim(),
                        Apellidos = Apellidos.Trim(),

                        Username = Username.Trim(),

                        RolId = RolSeleccionado.RolId,

                        Estado = Estado
                    };

                var res =
                    await _apiUsuario.ActualizarUsuarioAsync(
                        Usuario.UsuarioId,
                        dto);

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al actualizar usuario");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Usuario actualizado");
            }


            else
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    _notify.Warning(
                        "La contraseña es obligatoria");

                    return;
                }

                var dto =
                    new CrearUsuarioDto
                    {
                        Nombre = Nombre.Trim(),

                        Apellidos = Apellidos.Trim(),

                        Username = Username.Trim(),

                        Password = Password.Trim(),

                        RolId = RolSeleccionado.RolId,

                        Estado = Estado
                    };

                var res =
                    await _apiUsuario.CrearUsuarioAsync(dto);

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al crear usuario");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Usuario creado");
            }

            Limpiar();

            Close(true);
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

        Close(false);
    }
    public void Close(bool result = false)
    {
        _tcs.TrySetResult(result);
    }

    private void Limpiar()
    {
        Usuario = null;

        Codigo = "";

        Nombre = "";
        Apellidos = "";

        Username = "";
        Password = "";

        Estado = true;

        FechaRegistro = DateTime.Now;

        RolSeleccionado = null;
    }
}
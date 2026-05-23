using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Models;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

public partial class LoginOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly UsuarioApiService _usuarioApi;
    private readonly AppSesionViewModel _appSesion;
    private readonly NotificationService _notify;
    private readonly TurnoApiService _turnoApi;

    private TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;

    public LoginOverlayViewModel(
        UsuarioApiService usuarioApi,
        AppSesionViewModel appSesion,
        NotificationService notify,
        TurnoApiService turnoApi)
    {
        _usuarioApi = usuarioApi;
        _appSesion = appSesion;
        _notify = notify;
        _turnoApi = turnoApi;
    }

    [ObservableProperty]
    private string? username;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool estaAutenticado;


    [RelayCommand]
    public async Task<bool> LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            _notify.Warning("Debe ingresar usuario y contraseña");
            return false;
        }

        try
        {
            IsLoading = true;

            var dto = new LoginDto
            {
                Username = Username,
                Password = Password
            };

            var result = await _usuarioApi.LoginAsync(dto);

            if (!result.Success)
            {
                _notify.Error(result.Message);
                return false;
            }

            TokenStorage.SetToken(result.Data!.Token);
            _appSesion.IniciarSesion(result.Data!);

            await VerificarTurnoAsync();

            _notify.Success($"¡Bienvenido {result.Data!.UsuarioNombre}!");

            EstaAutenticado = true;

            Close(true);
            return true;
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }


    private async Task VerificarTurnoAsync()
    {
        var res = await _turnoApi.ObtenerTurnoActivoAsync();

        if (res.Success && res.Data != null)
        {
            _appSesion.TurnoActual = res.Data;
        }
    }

    private void Cancelar()
    {
        Close(false);
    }
    public void Close(bool result)
    {
        _tcs.TrySetResult(result);
    }
    public void ResetTask()
    {
        _tcs = new TaskCompletionSource<bool>();
    }
}
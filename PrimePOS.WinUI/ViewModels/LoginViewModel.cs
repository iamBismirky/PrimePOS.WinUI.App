using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Models;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

public partial class LoginViewModel : ObservableObject
{
    private readonly UsuarioApiService _usuarioApi;
    private readonly AppSesionViewModel _appSesion;
    private readonly NotificationService _notify;

    public LoginViewModel(UsuarioApiService usuarioApi, AppSesionViewModel appSesion, NotificationService notify)
    {
        _usuarioApi = usuarioApi;
        _appSesion = appSesion;
        _notify = notify;
    }

    [ObservableProperty]
    private string? username;
    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private bool isLoading;

    // LOGIN COMMAND
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
        {
            _notify.Warning("Debe ingresar usuario y contraseña");
            return;
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
                return;
            }
            TokenStorage.SetToken(result.Data!.Token);
            _appSesion.IniciarSesion(result.Data!);
            _notify.Success("¡Bienvenido " + result.Data!.UsuarioNombre + "!");
            //Navegación
            LoginSuccess?.Invoke();
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

    // Eventos para UI
    public event Action? LoginSuccess;
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

public partial class LoginViewModel : ObservableObject
{
    private readonly UsuarioApiService _usuarioApi;
    private readonly AppSesionViewModel _appSesion;

    public LoginViewModel(UsuarioApiService usuarioApi, AppSesionViewModel appSesion)
    {
        _usuarioApi = usuarioApi;
        _appSesion = appSesion;
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
        try
        {
            IsLoading = true;
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ErrorOcurrido?.Invoke("Debe ingresar usuario y contraseña");
                return;
            }
            var dto = new AutenticarUsuarioDto
            {
                Username = Username,
                Password = Password
            };

            //var result = await _usuarioApi.LoginAsync(dto);

            // Guardar sesión
            //_appSesion.IniciarSesion(result);

            // Aplicar token
            //_usuarioApi.SetToken(result.Token);

            //Navegación
            LoginSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorOcurrido?.Invoke(ex.Message);

        }
        finally
        {
            IsLoading = false;
        }
    }

    // Eventos para UI
    public event Action? LoginSuccess;
    public event Action<string>? ErrorOcurrido;
}
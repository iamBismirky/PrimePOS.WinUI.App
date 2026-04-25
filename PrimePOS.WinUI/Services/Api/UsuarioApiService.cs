using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Usuario;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class UsuarioApiService : BaseApiService
{
    public UsuarioApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<UsuarioDto>>> ObtenerUsuariosAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/usuarios");
        return SendAsync<List<UsuarioDto>>(request);
    }
    public Task<ApiResponse<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/usuarios/{id}");
        return SendAsync<UsuarioDto>(request);
    }
    public Task<ApiResponse<object>> CrearUsuarioAsync(CrearUsuarioDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/usuarios")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/usuarios/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarUsuarioAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/usuarios/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarUsuarioAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/usuarios/{id}");
        return SendAsync<object>(request);
    }
    public async Task<ApiResponse<AppSesionUsuarioDto>> LoginAsync(LoginDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
        {
            Content = JsonContent.Create(dto)
        };

        return await SendAsync<AppSesionUsuarioDto>(request);
    }

    public Task<ApiResponse<object>> CambiarPasswordAsync(CambiarPasswordDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/usuarios/cambiar-password")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

}
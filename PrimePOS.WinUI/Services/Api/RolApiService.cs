using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Rol;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class RolApiService : BaseApiService
{
    public RolApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<RolDto>>> ObtenerRolesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/roles");
        return SendAsync<List<RolDto>>(request);
    }

    public Task<ApiResponse<object>> CrearRolAsync(RolDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/roles")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarRolAsync(int id, RolDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/roles/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarRolAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/roles/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarRolAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/roles/{id}");
        return SendAsync<object>(request);
    }
}
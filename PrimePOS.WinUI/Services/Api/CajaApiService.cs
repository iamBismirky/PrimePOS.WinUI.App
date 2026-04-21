using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Caja;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class CajaApiService : BaseApiService
{
    public CajaApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<CajaDto>>> ObtenerCajasAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/cajas");
        return SendAsync<List<CajaDto>>(request);
    }

    public Task<ApiResponse<object>> CrearCajaAsync(CajaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/cajas")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarCajaAsync(int id, CajaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/cajas/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarCajaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/cajas/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarCajaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/cajas/{id}");
        return SendAsync<object>(request);
    }
}
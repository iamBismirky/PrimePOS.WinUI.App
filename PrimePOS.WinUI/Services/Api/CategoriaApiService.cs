using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Categoria;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class CategoriaApiService : BaseApiService
{
    public CategoriaApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<CategoriaDto>>> ObtenerCategoriasAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/categorias");
        return SendAsync<List<CategoriaDto>>(request);
    }

    public Task<ApiResponse<object>> CrearCategoriaAsync(CategoriaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/categorias")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarCategoriaAsync(int id, CategoriaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/categorias/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarCategoriaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/categorias/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarCategoriaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/categorias/{id}");
        return SendAsync<object>(request);
    }
}
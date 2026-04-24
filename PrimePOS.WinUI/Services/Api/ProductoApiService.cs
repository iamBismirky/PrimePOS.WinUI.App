using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Producto;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class ProductoApiService : BaseApiService
{
    public ProductoApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<ProductoDto>>> ObtenerProductosAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/productos");
        return SendAsync<List<ProductoDto>>(request);
    }

    public Task<ApiResponse<object>> CrearProductoAsync(CrearProductoDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/productos")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarProductoAsync(int id, ActualizarProductoDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/productos/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarProductoAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/productos/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarProductoAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/productos/{id}");
        return SendAsync<object>(request);
    }
}
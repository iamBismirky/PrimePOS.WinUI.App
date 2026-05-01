using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Cliente;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class ClienteApiService : BaseApiService
{
    public ClienteApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    public Task<ApiResponse<List<ClienteDto>>> ObtenerClientesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/clientes");
        return SendAsync<List<ClienteDto>>(request);
    }

    public Task<ApiResponse<object>> CrearClienteAsync(ClienteDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/clientes")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> ActualizarClienteAsync(int id, ClienteDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/clientes/{id}")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> DesactivarClienteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/clientes/{id}/desactivar");
        return SendAsync<object>(request);
    }

    public Task<ApiResponse<object>> EliminarClienteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/clientes/{id}");
        return SendAsync<object>(request);
    }
    public Task<ApiResponse<ClienteDto>> ObtenerClientePorIdAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/clientes/{id}");
        return SendAsync<ClienteDto>(request);
    }
    public Task<ApiResponse<List<ClienteDto>>> BuscarClientesAsync(string texto)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/clientes/buscar?texto={Uri.EscapeDataString(texto)}"
        );

        return SendAsync<List<ClienteDto>>(request);
    }
}
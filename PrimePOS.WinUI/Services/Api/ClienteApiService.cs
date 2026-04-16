using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Services.Api;

public class ClienteApiService
{
    private readonly HttpClient _http;

    public ClienteApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<List<ClienteDto>> ObtenerClientesAsync()
    {
        return await _http.GetFromJsonAsync<List<ClienteDto>>("api/clientes")
               ?? new List<ClienteDto>();
    }

    public async Task CrearClienteAsync(CrearClienteDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/clientes", dto);

        if (!res.IsSuccessStatusCode)
        {
            var json = await res.Content.ReadAsStringAsync();

            try
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(json);

                throw new Exception(error?.Message ?? "Error desconocido");
            }
            catch
            {
                throw new Exception(json);
            }
        }
    }

    public async Task ActualizarClienteAsync(int id, ActualizarClienteDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/clientes/{id}", dto);

        if (!res.IsSuccessStatusCode)
        {
            var json = await res.Content.ReadAsStringAsync();

            try
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(json);

                throw new Exception(error?.Message ?? "Error desconocido");
            }
            catch
            {
                throw new Exception(json);
            }
        }
    }

    public async Task DesactivarClienteAsync(int id)
    {
        var res = await _http.PatchAsync($"api/clientes/{id}/desactivar", null);

        if (!res.IsSuccessStatusCode)
        {
            var json = await res.Content.ReadAsStringAsync();

            try
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(json);

                throw new Exception(error?.Message ?? "Error desconocido");
            }
            catch
            {
                throw new Exception(json);
            }
        }
    }
}
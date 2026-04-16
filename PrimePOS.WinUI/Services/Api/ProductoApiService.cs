using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Services.Api;

public class ProductoApiService
{
    private readonly HttpClient _http;

    public ProductoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<List<ProductoDto>> ObtenerProductosAsync()
    {
        return await _http.GetFromJsonAsync<List<ProductoDto>>("api/productos")
               ?? new List<ProductoDto>();
    }

    public async Task CrearProductoAsync(CrearProductoDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/productos", dto);

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

    public async Task ActualizarProductoAsync(int id, ActualizarProductoDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/productos/{id}", dto);

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

    public async Task DesactivarProductoAsync(int id)
    {
        var res = await _http.PatchAsync($"api/productos/{id}/desactivar", null);

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
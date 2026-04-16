using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class CategoriaApiService
{
    private readonly HttpClient _http;

    public CategoriaApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<List<CategoriaDto>> ObtenerCategoriasAsync()
    {
        return await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias")
               ?? new List<CategoriaDto>();
    }

    public async Task CrearCategoriaAsync(CategoriaDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/categorias", dto);

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

    public async Task ActualizarCategoriaAsync(int id, CategoriaDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/categorias/{id}", dto);

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

    public async Task DesactivarCategoriaAsync(int id)
    {
        var res = await _http.PatchAsync($"api/categorias/{id}/desactivar", null);

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
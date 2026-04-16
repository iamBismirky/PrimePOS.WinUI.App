using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class CajaApiService
{
    private readonly HttpClient _http;

    public CajaApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<List<CajaDto>> ObtenerCajasAsync()
    {
        return await _http.GetFromJsonAsync<List<CajaDto>>("api/cajas")
               ?? new List<CajaDto>();
    }

    public async Task CrearCajaAsync(CajaDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/cajas", dto);

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

    public async Task ActualizarCajaAsync(int id, CajaDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/cajas/{id}", dto);

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

    public async Task DesactivarCajaAsync(int id)
    {
        var res = await _http.PatchAsync($"api/cajas/{id}/desactivar", null);

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
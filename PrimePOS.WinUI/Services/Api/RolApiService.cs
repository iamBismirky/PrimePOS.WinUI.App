using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class RolApiService
{
    private readonly HttpClient _http;

    public RolApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RolDto>> GetRolesAsync()
    {
        return await _http.GetFromJsonAsync<List<RolDto>>("api/roles") ?? new List<RolDto>();
    }

    public async Task CrearRolAsync(CrearRolDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/roles", dto);

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

    public async Task ActualizarRolAsync(int id, ActualizarRolDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/roles/{id}", dto);

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

    public async Task DesactivarRolAsync(int id)
    {
        var res = await _http.PatchAsync($"api/roles/{id}/desactivar", null);

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
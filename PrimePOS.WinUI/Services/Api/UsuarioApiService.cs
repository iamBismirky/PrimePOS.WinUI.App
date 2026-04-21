using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Services.Api;

public class UsuarioApiService
{
    private readonly HttpClient _http;

    public UsuarioApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<List<UsuarioDto>> ObtenerUsuariosAsync()
    {
        return await _http.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios")
               ?? new List<UsuarioDto>();
    }

    public async Task CrearUsuarioAsync(CrearUsuarioDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/usuarios", dto);

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

    public async Task ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/usuarios/{id}", dto);

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

    public async Task DesactivarUsuarioAsync(int id)
    {
        var res = await _http.PatchAsync($"api/usuarios/{id}/desactivar", null);

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
    public async Task<AppSesionUsuarioDto> LoginAsync(AutenticarUsuarioDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", dto);

        if (!res.IsSuccessStatusCode)
        {
            var json = await res.Content.ReadAsStringAsync();

            try
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                throw new Exception(error?.Message ?? "Credenciales incorrectas");
            }
            catch
            {
                throw new Exception("Credenciales incorrectas");
            }
        }

        var result = await res.Content.ReadFromJsonAsync<AppSesionUsuarioDto>();

        if (result == null)
            throw new Exception("Respuesta inválida del servidor");

        return result;
    }
    public void SetToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
    public async Task CambiarPasswordAsync(CambiarPasswordDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/usuarios/cambiar-password", dto);

        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ErrorResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            throw new Exception(error?.Message ?? "Error al cambiar contraseña");
        }
    }
}
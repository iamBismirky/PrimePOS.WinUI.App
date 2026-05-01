using PrimePOS.Contracts.Common;
using PrimePOS.WinUI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class BaseApiService
{
    protected readonly HttpClient _http;

    public static Action? OnUnauthorized;
    public BaseApiService(HttpClient http)
    {
        _http = http;
    }

    protected async Task<ApiResponse<T>> SendAsync<T>(HttpRequestMessage request)
    {
        try
        {
            var token = TokenStorage.GetToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            // 🔐 SOLO manejar 401 (infraestructura)
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                TokenStorage.Clear();
                OnUnauthorized?.Invoke();

                // ⚠️ Aquí SÍ creas uno porque el backend no lo manejó
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Sesión expirada"
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            // ⚠️ Si el backend respondió → usarlo SIEMPRE
            if (!string.IsNullOrWhiteSpace(content))
            {
                var result = JsonSerializer.Deserialize<ApiResponse<T>>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result != null)
                    return result; // 🔥 ESTE es el mensaje real del BLL
            }

            // ⚠️ SOLO si el backend NO respondió correctamente
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Respuesta inválida del servidor"
            };
        }
        catch (HttpRequestException)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Error de conexión"
            };
        }
        catch (Exception)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Error inesperado"
            };
        }
    }
}
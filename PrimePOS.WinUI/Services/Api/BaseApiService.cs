using PrimePOS.Contracts.Common;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class BaseApiService
{
    protected readonly HttpClient _http;

    public BaseApiService(HttpClient http)
    {
        _http = http;
    }

    protected async Task<ApiResponse<T>> SendAsync<T>(HttpRequestMessage request)
    {
        try
        {
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("RESPONSE API:");
            System.Diagnostics.Debug.WriteLine(content);
            System.Diagnostics.Debug.WriteLine("STATUS: " + response.StatusCode);
            // Intentar leer respuesta del backend (éxito o error)
            ApiResponse<T>? result = null;

            if (!string.IsNullOrWhiteSpace(content))
            {
                result = JsonSerializer.Deserialize<ApiResponse<T>>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            // ❌ Si no se pudo deserializar
            if (result == null)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = content ?? "Respuesta inválida del servidor"
                };
            }

            // ❌ Si HTTP fue error (400, 500, etc)
            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = result.Message ?? $"Error HTTP: {(int)response.StatusCode}"
                };
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Error de conexión: " + ex.Message
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Error inesperado: " + ex.Message
            };
        }
    }
}
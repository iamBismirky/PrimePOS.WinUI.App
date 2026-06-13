using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Empresa;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class EmpresaApiService : BaseApiService
{
    public EmpresaApiService(IHttpClientFactory factory)
    : base(factory.CreateClient("ApiClient"))
    {
    }
    public Task<ApiResponse<List<EmpresaDto>>> ObtenerEmpresasAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/empresas");
        return SendAsync<List<EmpresaDto>>(request);
    }
    public Task<ApiResponse<EmpresaDto>> ObtenerEmpresaPorIdAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/empresas/{id}");
        return SendAsync<EmpresaDto>(request);
    }
    public Task<ApiResponse<int>> CrearEmpresaAsync(CrearEmpresaDto dto)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/empresas")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<int>(request);
    }
    public async Task<ApiResponse<object>> SubirLogoAsync(int empresaId, Stream stream, string fileName)
    {
        var form = new MultipartFormDataContent();

        form.Add(
            new StreamContent(stream),
            "logo",
            fileName);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/empresas/{empresaId}/logo")
        {
            Content = form
        };

        return await SendAsync<object>(request);
    }
    public Task<ApiResponse<object>> ActualizarEmpresaAsync(int id, ActualizarEmpresaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/empresas/{id}")
        {
            Content = JsonContent.Create(dto)
        };
        return SendAsync<object>(request);

    }
    public async Task<ApiResponse<object>> EliminarEmpresaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/empresas/{id}");
        return await SendAsync<object>(request);
    }
    public Task<ApiResponse<List<EmpresaDto>>> BuscarEmpresasAsync(string texto)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/empresas/buscar?texto={texto}");
        return SendAsync<List<EmpresaDto>>(request);
    }
    public Task<ApiResponse<object>> DesactivarEmpresaAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/empresas/{id}/desactivar");
        return SendAsync<object>(request);
    }
}
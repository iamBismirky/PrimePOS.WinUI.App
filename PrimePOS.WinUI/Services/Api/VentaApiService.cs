using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Venta;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class VentaApiService : BaseApiService
{
    public VentaApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    // 🔹 CREAR VENTA
    public Task<ApiResponse<int>> CrearVentaAsync(CrearVentaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/venta")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<int>(request);
    }

    // 🔹 VENTAS POR TURNO
    public Task<ApiResponse<List<VentaDto>>> ObtenerPorTurnoAsync(int turnoId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/venta/por-turno/{turnoId}");

        return SendAsync<List<VentaDto>>(request);
    }

    // 🔹 VENTAS DEL DÍA
    public Task<ApiResponse<List<VentaDto>>> ObtenerVentasHoyAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/venta/hoy");
        return SendAsync<List<VentaDto>>(request);
    }
}
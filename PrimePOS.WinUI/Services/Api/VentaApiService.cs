using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Venta;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class VentaApiService : BaseApiService
{
    public VentaApiService(IHttpClientFactory factory) : base(factory.CreateClient("ApiClient")) { }


    // 🔹 CREAR VENTA
    public Task<ApiResponse<VentaConFacturaDto>> CrearVentaAsync(CrearVentaDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/venta")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<VentaConFacturaDto>(request);
    }

    // 🔹 VENTAS POR TURNO
    public Task<ApiResponse<decimal>> ObtenerTotalVentasPorTurnoAsync(int turnoId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/venta/ventas-turno/{turnoId}");

        return SendAsync<decimal>(request);
    }

    // 🔹 VENTAS DEL DÍA
    public Task<ApiResponse<List<VentaDto>>> ObtenerVentasHoyAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/venta/hoy");
        return SendAsync<List<VentaDto>>(request);
    }
    public Task<ApiResponse<List<ProductoVentaDto>>> BuscarProductosAsync(
    string texto,
    int tipoPrecioId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/venta/buscar-productos?" +
            $"texto={Uri.EscapeDataString(texto)}" +
            $"&tipoPrecioId={tipoPrecioId}");

        return SendAsync<List<ProductoVentaDto>>(request);
    }
    public Task<ApiResponse<List<ClienteVentaDto>>> BuscarClientesAsync(string texto)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/venta/buscar-clientes?" +
            $"texto={Uri.EscapeDataString(texto)}");

        return SendAsync<List<ClienteVentaDto>>(request);
    }
    public Task<ApiResponse<ClienteVentaDto>> CargarConsumidorFinalAsync()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/venta/cargar-consumidor-final");
        return SendAsync<ClienteVentaDto>(request);
    }

    public Task<ApiResponse<List<ProductoVentaDto>>> RecalcularProductosAsync(
    List<int> productoIds,
    int tipoPrecioId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/venta/recalcular-productos?" +
            $"tipoPrecioId={tipoPrecioId}")
        {
            Content = JsonContent.Create(productoIds)
        };

        return SendAsync<List<ProductoVentaDto>>(request);
    }
}
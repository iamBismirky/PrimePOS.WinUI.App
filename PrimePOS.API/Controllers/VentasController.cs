using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Venta;
using System.Security.Claims;

namespace PrimePOS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class VentaController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentaController(IVentaService service)
    {
        _ventaService = service;
    }

    // 🔹 CREAR VENTA
    [HttpPost]
    public async Task<IActionResult> CrearVentaAsync([FromBody] CrearVentaDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var nombre = User.Identity!.Name;
        var ventaResponse = await _ventaService.CrearVentaAsync(userId, nombre!, dto);

        return Ok(new ApiResponse<VentaConFacturaDto>
        {
            Success = true,
            Data = ventaResponse,
            Message = "Venta registrada correctamente"
        });
    }

    // 🔹 VENTAS POR TURNO
    [HttpGet("ventas-turno/{turnoId}")]
    public async Task<IActionResult> ObtenerPorTurnoAsync(int turnoId)
    {
        var ventas = await _ventaService.ObtenerVentasPorTurnoAsync(turnoId);

        return Ok(new ApiResponse<decimal>
        {
            Success = true,
            Data = ventas
        });
    }

    // 🔹 VENTAS DEL DÍA
    [HttpGet("hoy")]
    public async Task<IActionResult> ObtenerVentasHoyAsync()
    {
        var ventas = await _ventaService.ObtenerVentasDelDiaAsync();

        return Ok(new ApiResponse<List<VentaDto>>
        {
            Success = true,
            Data = ventas
        });
    }

    [HttpGet("buscar-productos")]
    public async Task<IActionResult> BuscarProductosAsync(
    [FromQuery] string texto,
    [FromQuery] int tipoPrecioId)
    {
        var data = await _ventaService.BuscarProductosAsync(
            texto,
            tipoPrecioId);

        return Ok(new ApiResponse<List<ProductoVentaDto>>
        {
            Success = true,
            Data = data
        });
    }
    [HttpGet("buscar-clientes")]
    public async Task<IActionResult> BuscarClientesAsync([FromQuery] string texto)
    {
        var data = await _ventaService.BuscarClientesAsync(
            texto);

        return Ok(new ApiResponse<List<ClienteVentaDto>>
        {
            Success = true,
            Data = data
        });
    }
    [HttpGet("cargar-consumidor-final")]
    public async Task<IActionResult> CargarConsumidorFinalAsync()
    {
        var data = await _ventaService.CargarConsumidorFinalAsync();

        return Ok(new ApiResponse<ClienteVentaDto>
        {
            Success = true,
            Data = data
        });
    }
    [HttpPost("recalcular-productos")]
    public async Task<IActionResult> RecalcularProductosAsync([FromBody] List<int> productoIds, [FromQuery] int tipoPrecioId)
    {
        var data = await _ventaService.RecalcularProductosAsync(
            productoIds,
            tipoPrecioId);

        return Ok(new ApiResponse<List<ProductoVentaDto>>
        {
            Success = true,
            Data = data
        });
    }
}
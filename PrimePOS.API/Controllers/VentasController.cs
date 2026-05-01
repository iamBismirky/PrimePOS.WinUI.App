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
    private readonly IVentaService _service;

    public VentaController(IVentaService service)
    {
        _service = service;
    }

    // 🔹 CREAR VENTA
    [HttpPost]
    public async Task<IActionResult> CrearVentaAsync([FromBody] CrearVentaDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var nombre = User.Identity!.Name;
        var ventaResponse = await _service.CrearVentaAsync(userId, nombre!, dto);

        var urlPdf = $"{Request.Scheme}://{Request.Host}/facturas/{ventaResponse.FileName}";
        ventaResponse.UrlPdf = urlPdf;
        return Ok(new ApiResponse<VentaResponseDto>
        {
            Success = true,
            Data = ventaResponse,
            Message = "Venta registrada correctamente"
        });
    }

    // 🔹 VENTAS POR TURNO
    [HttpGet("por-turno/{turnoId}")]
    public async Task<IActionResult> ObtenerPorTurnoAsync(int turnoId)
    {
        var ventas = await _service.ObtenerVentasPorTurnoAsync(turnoId);

        return Ok(new ApiResponse<List<VentaDto>>
        {
            Success = true,
            Data = ventas
        });
    }

    // 🔹 VENTAS DEL DÍA
    [HttpGet("hoy")]
    public async Task<IActionResult> ObtenerVentasHoyAsync()
    {
        var ventas = await _service.ObtenerVentasDelDiaAsync();

        return Ok(new ApiResponse<List<VentaDto>>
        {
            Success = true,
            Data = ventas
        });
    }
}
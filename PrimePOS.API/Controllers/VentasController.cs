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

        dto.UsuarioId = userId;

        var ventaId = await _service.CrearVentaAsync(dto);

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Data = ventaId,
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
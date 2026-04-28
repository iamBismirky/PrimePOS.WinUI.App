using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Turno;
using System.Security.Claims;

namespace PrimePOS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly ITurnoService _service;

    public TurnosController(ITurnoService service)
    {
        _service = service;
    }

    [HttpPost("abrir")]
    public async Task<IActionResult> CrearTurnoAsync([FromBody] CrearTurnoDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        dto.UsuarioId = userId;
        var turno = await _service.CrearTurnoDtoAsync(dto);
        return Ok(new ApiResponse<TurnoDto>
        {
            Success = true,
            Data = turno,
            Message = "Turno abierto correctamente"
        });
    }


    [HttpPost("cerrar")]
    public async Task<IActionResult> CerrarTurnoAsync([FromBody] CierreTurnoDto dto)
    {
        await _service.CerrarTurnoAsync(dto);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Turno cerrado correctamente"
        });
    }

    [HttpGet("resumen/{id}")]
    public async Task<IActionResult> ObtenerResumenTurnoAsync(int id)
    {
        var resumen = await _service.ObtenerResumenTurno(id);

        return Ok(new ApiResponse<CierreTurnoDto>
        {
            Success = true,
            Data = resumen,
            Message = "Resumen del turno obtenido correctamente"
        });
    }

    // 🔹 SIGUIENTE TURNO
    [HttpGet("siguiente")]
    public async Task<IActionResult> ObtenerSiguienteTurnoAsync()
    {
        var numero = await _service.ObtenerSiguienteTurno();

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Data = numero
        });
    }


    // 🔹 TURNO ABIERTO (CAJA + USUARIO)
    [HttpGet("activo/{cajaId}")]
    public async Task<IActionResult> ObtenerTurnoActivo(int cajaId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Usuario no autenticado"
            });
        }

        var userId = int.Parse(userIdClaim.Value);
        var turno = await _service.ObtenerTurnoAbiertoAsync(cajaId, userId);

        return Ok(new ApiResponse<TurnoDto?>
        {
            Success = true,
            Data = turno
        });
    }
}
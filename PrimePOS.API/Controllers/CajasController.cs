using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Caja;

namespace PrimePOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CajasController : ControllerBase
{
    private readonly ICajaService _service;

    public CajasController(ICajaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var cajas = await _service.ListarCajasAsync();

        return Ok(new ApiResponse<List<CajaDto>>
        {
            Success = true,
            Data = cajas
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerCajaPorIdAsync(int id)
    {
        var caja = await _service.ObtenerCajaPorIdAsync(id);

        return Ok(new ApiResponse<CajaDto>
        {
            Success = true,
            Data = caja
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearCajaAsync([FromBody] CajaDto dto)
    {
        await _service.CrearCajaAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Caja creada correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarCajaAsync(int id, [FromBody] CajaDto dto)
    {
        dto.CajaId = id;

        await _service.ActualizarCajaAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Caja actualizada correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarCajaAsync(int id)
    {
        await _service.DesactivarCajaAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Caja desactivada correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCajaAsync(int id)
    {
        await _service.EliminarCajaAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Caja eliminada correctamente"
        });
    }
}
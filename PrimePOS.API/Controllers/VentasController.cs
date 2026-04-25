using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;

namespace PrimePOS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _service;

    public VentasController(IVentaService service)
    {
        _service = service;
    }

    //[HttpGet]
    //public async Task<IActionResult> ObtenerTodosAsync()
    //{
    //    var ventas = await _service.ObtenerVentasDelDiaAsync();

    //    return Ok(new ApiResponse<List<VentaDto>>
    //    {
    //        Success = true,
    //        Data = ventas
    //    });
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> ObtenerPorIdAsync(int id)
    //{
    //    var venta = await _service.ob(id);

    //    return Ok(new ApiResponse<VentaDto>
    //    {
    //        Success = true,
    //        Data = venta
    //    });
    //}

    //[HttpGet("turno/{turnoId}")]
    //public async Task<IActionResult> ObtenerPorTurnoAsync(int turnoId)
    //{
    //    var ventas = await _service.ListarPorTurnoAsync(turnoId);

    //    return Ok(new ApiResponse<List<VentaDto>>
    //    {
    //        Success = true,
    //        Data = ventas
    //    });
    //}

    //[HttpPost]
    //public async Task<IActionResult> CrearAsync([FromBody] CrearVentaDto dto)
    //{
    //    var id = await _service.CrearVentaAsync(dto);

    //    return Ok(new ApiResponse<int>
    //    {
    //        Success = true,
    //        Data = id,
    //        Message = "Venta registrada correctamente"
    //    });
    //}

    //[HttpPost("{id}/anular")]
    //public async Task<IActionResult> AnularAsync(int id)
    //{
    //    await _service.AnularVentaAsync(id);

    //    return Ok(new ApiResponse<object>
    //    {
    //        Success = true,
    //        Message = "Venta anulada correctamente"
    //    });
    //}
}
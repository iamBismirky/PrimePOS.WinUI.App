using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;

namespace PrimePOS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly IFacturaService _service;

    public FacturaController(IFacturaService service)
    {
        _service = service;
    }

    //// 🔹 GENERAR FACTURA DESDE VENTA
    //[HttpPost("generar/{ventaId}")]
    //public async Task<IActionResult> GenerarFacturaAsync(int ventaId)
    //{
    //    var factura = await _service.GenerarFacturaDesdeVenta(ventaId);

    //    var dto = _service.MapearFactura(factura);

    //    return Ok(new ApiResponse<FacturaDto>
    //    {
    //        Success = true,
    //        Data = dto,
    //        Message = "Factura generada correctamente"
    //    });
    //}

    // 🔹 ANULAR FACTURA
    [HttpPatch("anular/{facturaId}")]
    public async Task<IActionResult> AnularFacturaAsync(int facturaId)
    {
        await _service.AnularFactura(facturaId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Factura anulada correctamente"
        });
    }
}
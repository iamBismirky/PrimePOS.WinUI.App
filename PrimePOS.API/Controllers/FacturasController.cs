using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Factura;

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

    // 🔹 GENERAR FACTURA DESDE VENTA
    [HttpPost("generar/{ventaId}")]
    public async Task<IActionResult> GenerarFacturaAsync(int ventaId)
    {
        var result = await _service.GenerarFacturaDesdeVenta(ventaId);

        return Ok(new ApiResponse<FacturaGeneradaDto>
        {
            Success = true,
            Message = "Factura generada correctamente",
            Data = result

        });
    }

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
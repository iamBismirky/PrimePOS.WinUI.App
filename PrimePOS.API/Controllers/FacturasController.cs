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
    private readonly IPdfService _pdfService;

    public FacturaController(IFacturaService service, IPdfService pdfService)
    {
        _service = service;
        _pdfService = pdfService;
    }

    // 🔹 GENERAR FACTURA DESDE VENTA
    //[HttpPost("generar/{ventaId}")]
    //public async Task<IActionResult> GenerarFacturaAsync(int ventaId)
    //{
    //    var result = await _service.GenerarFacturaDesdeVenta(ventaId);

    //    return Ok(new ApiResponse<FacturaGeneradaDto>
    //    {
    //        Success = true,
    //        Message = "Factura generada correctamente",
    //        Data = result

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
    [HttpGet("pdf/{fileName}")]
    public IActionResult DescargarPdf(string fileName)
    {
        var path = _pdfService.GetFilePath(fileName);

        var bytes = System.IO.File.ReadAllBytes(path);

        Response.Headers["Content-Disposition"] = "inline";

        return File(bytes, "application/pdf");
    }
    [HttpGet("todas")]
    public async Task<IActionResult> ObtenerTodasAsync()
    {
        var facturas = await _service.ObtenerTodosAsync();

        return Ok(new ApiResponse<List<FacturaListadoDto>>
        {
            Success = true,
            Message = "Facturas obtenidas correctamente",
            Data = facturas
        });
    }
}
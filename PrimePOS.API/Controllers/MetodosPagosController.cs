using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.MetodoPago;

namespace PrimePOS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MetodoPagoController : ControllerBase
{
    private readonly IMetodoPagoService _service;

    public MetodoPagoController(IMetodoPagoService service)
    {
        _service = service;
    }

    // 🔹 LISTAR MÉTODOS DE PAGO
    [HttpGet]
    public async Task<IActionResult> ObtenerMetodosPagoAsync()
    {
        var metodos = await _service.ListarMetodosPagosAsync();

        return Ok(new ApiResponse<List<MetodoPagoDto>>
        {
            Success = true,
            Data = metodos,
            Message = "Métodos de pago obtenidos correctamente"
        });
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.MetodoPago;

namespace PrimePOS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _service;
    public CatalogController(ICatalogService service) { _service = service; }


    [HttpGet("tipos-clientes")]
    public async Task<IActionResult> ObtenerTodosTipoClientes()
    {
        var tipos = await _service.ObtenerTodosTipoClientesAsync();

        return Ok(new ApiResponse<List<TipoClienteDto>>
        {
            Success = true,
            Data = tipos,
            Message = "Tipos de clientes obtenidos correctamente"
        });
    }
    [HttpGet("tipos-ventas")]
    public async Task<IActionResult> ObtenerTodosTipoVentas()
    {
        var tipos = await _service.ObtenerTodosTipoVentaAsync();

        return Ok(new ApiResponse<List<TipoVentaDto>>
        {
            Success = true,
            Data = tipos,
            Message = "Tipos de ventas obtenidos correctamente"
        });
    }
    [HttpGet("estados-ventas")]
    public async Task<IActionResult> ObtenerTodosEstadoVentas()
    {
        var Estados = await _service.ObtenerTodosEstadoVentaAsync();

        return Ok(new ApiResponse<List<EstadoVentaDto>>
        {
            Success = true,
            Data = Estados,
            Message = "Estados ventas obtenidos correctamente"
        });
    }
    [HttpGet("metodos-pagos")]
    public async Task<IActionResult> ObtenerTodosMetodoPagos()
    {
        var metodos = await _service.ObtenerTodosMetodosPagosAsync();

        return Ok(new ApiResponse<List<MetodoPagoDto>>
        {
            Success = true,
            Data = metodos,
            Message = "Metodos de pagos obtenidos correctamente"
        });
    }

}
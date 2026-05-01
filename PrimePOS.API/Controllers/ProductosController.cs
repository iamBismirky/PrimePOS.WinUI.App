using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductosController(IProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var productos = await _service.ObtenerTodosAsync();

        return Ok(new ApiResponse<List<ProductoDto>>
        {
            Success = true,
            Data = productos
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerProductoPorIdAsync(int id)
    {
        var producto = await _service.ObtenerPorIdAsync(id);

        return Ok(new ApiResponse<ProductoDto>
        {
            Success = true,
            Data = producto
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearProductoAsync([FromBody] CrearProductoDto dto)
    {
        await _service.CrearProductoAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Producto creado correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProductoAsync(int id, [FromBody] ActualizarProductoDto dto)
    {
        dto.ProductoId = id;

        await _service.ActualizarProductoAsync(dto);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Producto actualizado correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarProductoAsync(int id)
    {
        await _service.DesactivarProductoAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Producto desactivado correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProductoAsync(int id)
    {
        await _service.EliminarProductoAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Producto eliminado correctamente"
        });
    }
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string texto)
    {
        var data = await _service.BuscarProductosAsync(texto);
        return Ok(new ApiResponse<List<ProductoDto>>
        {
            Success = true,
            Data = data
        });

    }
}
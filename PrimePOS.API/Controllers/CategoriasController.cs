using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Categoria;

namespace PrimePOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var categorias = await _service.ListarCategoriasAsync();

        return Ok(new ApiResponse<List<CategoriaDto>>
        {
            Success = true,
            Data = categorias
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerCategoriaPorIdAsync(int id)
    {
        var categoria = await _service.ObtenerPorIdAsync(id);

        return Ok(new ApiResponse<CategoriaDto>
        {
            Success = true,
            Data = categoria
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearCategoriaAsync([FromBody] CategoriaDto dto)
    {
        await _service.CrearCategoriaAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Categoria creada correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarCategoriaAsync(int id, [FromBody] CategoriaDto dto)
    {
        dto.CategoriaId = id;

        await _service.ActualizarCategoriaAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Categoria actualizada correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarCategoriaAsync(int id)
    {
        await _service.DesactivarCategoriaAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Categoria desactivada correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCategoriaAsync(int id)
    {
        await _service.EliminarCategoriaAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Categoria eliminada correctamente"
        });
    }
}
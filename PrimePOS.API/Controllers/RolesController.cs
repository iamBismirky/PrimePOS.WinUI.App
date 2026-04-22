using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Rol;

namespace PrimePOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolService _service;

    public RolesController(IRolService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var roles = await _service.ListarRolesAsync();

        return Ok(new ApiResponse<List<RolDto>>
        {
            Success = true,
            Data = roles
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerRolPorIdAsync(int id)
    {
        var rol = await _service.ObtenerRolPorIdAsync(id);

        return Ok(new ApiResponse<RolDto>
        {
            Success = true,
            Data = rol
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearRolAsync([FromBody] CrearRolDto dto)
    {
        await _service.CrearRolAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Rol creado correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarRolAsync(int id, [FromBody] ActualizarRolDto dto)
    {
        dto.RolId = id;

        await _service.ActualizarRolAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Rol actualizado correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarRolAsync(int id)
    {
        await _service.DesactivarRolAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Rol desactivado correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarRolAsync(int id)
    {
        await _service.EliminarRolAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Rol eliminado correctamente"
        });
    }
}
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
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

    // GET: api/roles
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var roles = await _service.ListarRolesAsync();
        return Ok(roles);
    }

    // GET: api/roles/5
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorIdAsync(int id)
    {
        var rol = await _service.ObtenerRolPorIdAsync(id);
        return Ok(rol);
    }

    // POST: api/roles
    [HttpPost]
    public async Task<IActionResult> CrearRolAsync([FromBody] CrearRolDto dto)
    {
        await _service.CrearRolAsync(dto);

        return StatusCode(201, new
        {
            message = "Rol creado correctamente"
        });
    }

    // PUT: api/roles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarRolAsync(int id, [FromBody] ActualizarRolDto dto)
    {
        dto.RolId = id;

        await _service.ActualizarRolAsync(dto);

        return Ok(new
        {
            message = "Rol actualizado correctamente"
        });
    }

    // PATCH: api/roles/5/desactivar
    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarRolAsync(int id)
    {
        await _service.DesactivarRolAsync(id);

        return NoContent();
    }

    // DELETE: api/roles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarRolAsync(int id)
    {
        await _service.EliminarRolAsync(id);

        return NoContent();
    }
}
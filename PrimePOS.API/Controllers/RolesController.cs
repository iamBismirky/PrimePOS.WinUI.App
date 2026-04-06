using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.BLL.Interfaces;

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

    // 🔹 GET: api/roles
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var roles = await _service.ListarRolesAsync();
        return Ok(roles);
    }

    // 🔹 GET: api/roles/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var rol = await _service.ObtenerRolPorIdAsync(id);

        if (rol == null)
            return NotFound(new { mensaje = "Rol no encontrado" });

        return Ok(rol);
    }

    // 🔹 POST: api/roles
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CrearRolDto dto)
    {
        try
        {
            await _service.CrearRolAsync(dto);

            return Created("", new
            {
                mensaje = "Rol creado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // 🔹 PUT: api/roles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ActualizarRolDto dto)
    {
        try
        {
            dto.RolId = id; // 👈 importante

            await _service.ActualizarRolAsync(dto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // 🔹 PATCH: api/roles/5 (desactivar)
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] RolDto dto)
    {
        try
        {
            dto.RolId = id;

            await _service.DesactivarRolAsync(dto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // 🔹 DELETE: api/roles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var dto = new EliminarRolDto { RolId = id };

            await _service.EliminarRolAsync(dto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
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

    // 🔹 GET: api/roles
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var roles = await _service.ListarRolesAsync();
        return Ok(roles);
    }

    // 🔹 GET: api/roles/5
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorIdAsync(int id)
    {
        var rol = await _service.ObtenerRolPorIdAsync(id);

        if (rol == null)
            return NotFound(new { mensaje = "Rol no encontrado" });

        return Ok(rol);
    }


    [HttpPost]
    public async Task<IActionResult> CrearRolAsync([FromBody] CrearRolDto dto)
    {
        try
        {
            await _service.CrearRolAsync(dto);

            return Ok();
        }
        catch (BusinessException ex)
        {

            return BadRequest(new { message = ex.Message, code = ex.Code });
        }
    }

    // 🔹 PUT: api/roles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarRolAsync(int id, [FromBody] ActualizarRolDto dto)
    {
        try
        {
            dto.RolId = id;

            await _service.ActualizarRolAsync(dto);
            return Ok();
        }
        catch (BusinessException ex)
        {

            return BadRequest(new
            {
                message = ex.Message,
                code = ex.Code
            });

        }
    }

    // 🔹 PATCH: api/roles/5/desactivar
    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarRolAsync(int id)
    {
        try
        {
            await _service.DesactivarRolAsync(id);

            return Ok();
        }
        catch (BusinessException ex)
        {

            return BadRequest(new
            {
                message = ex.Message,
                code = ex.Code
            });

        }

    }

    // 🔹 DELETE: api/roles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarRolAsync(int id)
    {
        try
        {
            await _service.EliminarRolAsync(new EliminarRolDto
            {
                RolId = id
            });
            return Ok();
        }
        catch (BusinessException ex)
        {

            return BadRequest(new
            {
                message = ex.Message,
                code = ex.Code
            });

        }
    }
}
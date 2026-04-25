using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Usuario;
using System.Security.Claims;

namespace PrimePOS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var usuarios = await _service.ObtenerTodosAsync();

        return Ok(new ApiResponse<List<UsuarioDto>>
        {
            Success = true,
            Data = usuarios
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerUsuarioPorIdAsync(int id)
    {
        var usuario = await _service.ObtenerUsuarioPorIdAsync(id);
        return Ok(new ApiResponse<UsuarioDto>
        {
            Success = true,
            Data = usuario
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearUsuarioAsync([FromBody] CrearUsuarioDto dto)
    {
        await _service.CrearUsuarioAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario creado correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarUsuarioAsync(int id, [FromBody] ActualizarUsuarioDto dto)
    {
        dto.UsuarioId = id;

        await _service.ActualizarUsuarioAsync(dto);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario actualizado correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarUsuarioAsync(int id)
    {
        await _service.DesactivarUsuarioAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario desactivado correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarUsuarioAsync(int id)
    {
        await _service.EliminarUsuarioAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario eliminado correctamente"
        });
    }
    [HttpPost("cambiar-password")]
    public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _service.CambiarPasswordAsync(userId, dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Contraseña actualizada correctamente"
        });

    }
}
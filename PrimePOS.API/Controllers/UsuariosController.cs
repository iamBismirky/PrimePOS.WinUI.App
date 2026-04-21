using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Usuario;
using System.Security.Claims;

namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var usuarios = await _service.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var usuario = await _service.ObtenerUsuarioPorIdAsync(id);
            return Ok(usuario);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> CrearUsuarioAsync([FromBody] CrearUsuarioDto dto)
        {
            await _service.CrearUsuarioAsync(dto);

            return StatusCode(201, new
            {
                message = "Usuario creado correctamente"
            });
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuarioAsync(int id, [FromBody] ActualizarUsuarioDto dto)
        {
            dto.UsuarioId = id;

            await _service.ActualizarUsuarioAsync(dto);

            return Ok(new
            {
                message = "Usuario actualizado correctamente"
            });
        }

        // PATCH: api/usuarios/5/desactivar
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarUsuarioAsync(int id)
        {
            await _service.DesactivarUsuarioAsync(id);

            return NoContent();
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuarioAsync(int id)
        {
            await _service.EliminarUsuarioAsync(id);

            return NoContent();
        }

        // PATCH: api/usuarios/cambiar-password
        [Authorize]
        [HttpPatch("cambiar-password")]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordDto dto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.CambiarPasswordAsync(usuarioId, dto);

            return Ok(new
            {
                message = "Contraseña actualizada correctamente"
            });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Usuario;



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


        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var usuarios = await _service.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        //GET: 
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var usuario = await _service.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }


        [HttpPost]
        public async Task<IActionResult> CrearUsuarioAsync([FromBody] CrearUsuarioDto dto)
        {
            try
            {
                await _service.CrearUsuarioAsync(dto);
                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(new { message = ex.Message, code = ex.Code });
            }
        }

        // 🔹 PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuarioAsync(int id, [FromBody] ActualizarUsuarioDto dto)
        {
            try

            {
                dto.UsuarioId = id;

                await _service.ActualizarUsuarioAsync(dto);
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

        //PATCH
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarUsuarioAsync(int id)
        {
            try
            {
                await _service.DesactivarUsuarioAsync(id);
                return NoContent();
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

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuarioAsync(int id)
        {
            try
            {
                await _service.EliminarUsuarioAsync(id);
                return NoContent();
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
}


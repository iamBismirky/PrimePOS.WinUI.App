using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Cliente;


namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var cliente = await _service.ObtenerTodosAsync();
            return Ok(cliente);
        }

        //GET: 
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _service.ObtenerPorIdAsync(id);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }


        [HttpPost]
        public async Task<IActionResult> CrearClienteAsync([FromBody] CrearClienteDto dto)
        {
            try
            {
                await _service.CrearClienteAsync(dto);

                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(new { message = ex.Message, code = ex.Code });
            }
        }

        // 🔹 PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarClienteAsync(int id, [FromBody] ActualizarClienteDto dto)
        {
            try

            {
                dto.ClienteId = id;

                await _service.ActualizarClienteAsync(dto);
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
        public async Task<IActionResult> DesactivarCategoriaAsync(int id)
        {
            try
            {
                await _service.DesactivarClienteAsync(id);
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
        public async Task<IActionResult> EliminarClienteAsync(int id)
        {
            try
            {
                await _service.EliminarClienteAsync(id);
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


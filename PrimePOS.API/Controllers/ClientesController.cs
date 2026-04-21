using Microsoft.AspNetCore.Mvc;
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

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var clientes = await _service.ObtenerTodosAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _service.ObtenerPorIdAsync(id);
            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> CrearClienteAsync([FromBody] CrearClienteDto dto)
        {
            await _service.CrearClienteAsync(dto);

            return StatusCode(201, new
            {
                message = "Cliente creado correctamente"
            });
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarClienteAsync(int id, [FromBody] ActualizarClienteDto dto)
        {
            dto.ClienteId = id;

            await _service.ActualizarClienteAsync(dto);

            return Ok(new
            {
                message = "Cliente actualizado correctamente"
            });
        }

        // PATCH: api/clientes/5/desactivar
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarClienteAsync(int id)
        {
            await _service.DesactivarClienteAsync(id);

            return NoContent();
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarClienteAsync(int id)
        {
            await _service.EliminarClienteAsync(id);

            return NoContent();
        }
    }
}
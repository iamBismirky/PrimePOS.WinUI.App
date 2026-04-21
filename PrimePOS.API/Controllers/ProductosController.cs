using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var productos = await _service.ObtenerTodosAsync();
            return Ok(productos);
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var producto = await _service.ObtenerProductoPorIdAsync(id);
            return Ok(producto);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<IActionResult> CrearProductoAsync([FromBody] CrearProductoDto dto)
        {
            await _service.CrearProductoAsync(dto);

            return StatusCode(201, new
            {
                message = "Producto creado correctamente"
            });
        }

        // PUT: api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProductoAsync(int id, [FromBody] ActualizarProductoDto dto)
        {
            dto.ProductoId = id;

            await _service.ActualizarProductoAsync(dto);

            return Ok(new
            {
                message = "Producto actualizado correctamente"
            });
        }

        // PATCH: api/productos/5/desactivar
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarProductoAsync(int id)
        {
            await _service.DesactivarProductoAsync(id);

            return NoContent();
        }

        // DELETE: api/productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProductoAsync(int id)
        {
            await _service.EliminarProductoAsync(id);

            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Categoria;

namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> ObtenerCategoriasAsync()
        {
            var categorias = await _service.ListarCategoriasAsync();
            return Ok(categorias);
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var categoria = await _service.ObtenerPorIdAsync(id);
            return Ok(categoria);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> CrearCategoriaAsync([FromBody] CategoriaDto dto)
        {
            await _service.CrearCategoriaAsync(dto);

            return StatusCode(201, new
            {
                message = "Categoría creada correctamente"
            });
        }

        // PUT: api/categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoriaAsync(int id, [FromBody] CategoriaDto dto)
        {
            dto.CategoriaId = id;

            await _service.ActualizarCategoriaAsync(dto);

            return Ok(new
            {
                message = "Categoría actualizada correctamente"
            });
        }

        // PATCH: api/categorias/5/desactivar
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarCategoriaAsync(int id)
        {
            await _service.DesactivarCategoriaAsync(id);

            return NoContent();
        }

        // DELETE: api/categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoriaAsync(int id)
        {
            await _service.EliminarCategoriaAsync(id);

            return NoContent();
        }
    }
}
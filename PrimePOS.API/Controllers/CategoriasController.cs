using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
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


        [HttpGet]
        public async Task<IActionResult> ObtenerCategoriasAsync()
        {
            var categorias = await _service.ListarCategoriasAsync();
            return Ok(categorias);
        }

        // 🔹 GET: api/roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var categoria = await _service.ObtenerPorIdAsync(id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoria no encontrada" });

            return Ok(categoria);
        }


        [HttpPost]
        public async Task<IActionResult> CrearCategoriaAsync([FromBody] CategoriaDto dto)
        {
            try
            {
                await _service.CrearCategoriaAsync(dto);

                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(new { message = ex.Message, code = ex.Code });
            }
        }

        // 🔹 PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoriaAsync(int id, [FromBody] CategoriaDto dto)
        {
            try

            {
                dto.CategoriaId = id;

                await _service.ActualizarCategoriaAsync(dto);
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
                await _service.DesactivarCategoriaAsync(id);
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

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoriaAsync(int id)
        {
            try
            {
                await _service.EliminarCategoriaAsync(id);
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

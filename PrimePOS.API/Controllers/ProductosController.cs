using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
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


        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var productos = await _service.ObtenerTodosAsync();
            return Ok(productos);
        }

        //GET: 
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _service.ObtenerProductoPorIdAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });
            return Ok(producto);
        }


        [HttpPost]
        public async Task<IActionResult> CrearProductoAsync([FromBody] CrearProductoDto dto)
        {
            try
            {
                await _service.CrearProductoAsync(dto);

                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(new { message = ex.Message, code = ex.Code });
            }
        }

        // 🔹 PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProductoAsync(int id, [FromBody] ActualizarProductoDto dto)
        {
            try

            {
                dto.ProductoId = id;

                await _service.ActualizarProductoAsync(dto);
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
        public async Task<IActionResult> DesactivarProductoAsync(int id)
        {
            try
            {
                await _service.DesactivarProductoAsync(id);
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
        public async Task<IActionResult> EliminarProductoAsync(int id)
        {
            try
            {
                await _service.EliminarProductoAsync(id);
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


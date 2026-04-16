using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Caja;

namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CajasController : ControllerBase
    {
        private readonly ICajaService _service;

        public CajasController(ICajaService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerTodosAsync()
        {
            var caja = await _service.ListarCajasAsync();
            return Ok(caja);
        }

        //GET: 
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCajaPorIdAsync(int id)
        {
            var caja = await _service.ObtenerCajaPorIdAsync(id);

            if (caja == null)
                return NotFound(new { mensaje = "Rol no encontrado" });

            return Ok(caja);
        }


        [HttpPost]
        public async Task<IActionResult> CrearCajaAsync([FromBody] CajaDto dto)
        {
            try
            {
                await _service.CrearCajaAsync(dto);

                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(new { message = ex.Message, code = ex.Code });
            }
        }

        // 🔹 PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCajaAsync(int id, [FromBody] CajaDto dto)
        {
            try

            {
                dto.CajaId = id;

                await _service.ActualizarCajaAsync(dto);
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
                await _service.DesactivarCajaAsync(id);
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
        public async Task<IActionResult> EliminarCajaAsync(int id)
        {
            try
            {
                await _service.EliminarCajaAsync(id);
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


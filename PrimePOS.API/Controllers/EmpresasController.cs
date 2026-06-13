using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Empresa;

namespace PrimePOS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _service;

    public EmpresasController(IEmpresaService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerEmpresasAsync()
    {
        var empresas = await _service.ObtenerTodosAsync();


        return Ok(new ApiResponse<List<EmpresaDto>>
        {
            Success = true,
            Data = empresas
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerEmpresaPorIdAsync(int id)
    {
        var empresa = await _service.ObtenerPorIdAsync(id);
        return Ok(new ApiResponse<EmpresaDto>
        {
            Success = true,
            Data = empresa
        });
    }
    [HttpPost]
    public async Task<IActionResult> CrearEmpresaAsync([FromBody] CrearEmpresaDto dto)
    {
        var empresaId = await _service.CrearAsync(dto);

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Data = empresaId,
            Message = "Empresa creada exitosamente"
        });
    }

    [HttpPost("{empresaId}/logo")]
    public async Task<IActionResult> SubirLogoAsync(int empresaId, IFormFile logo)
    {
        await _service.SubirLogoAsync(
            empresaId,
            logo);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Logo actualizado"
        });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarEmpresaAsync(int id, ActualizarEmpresaDto dto)
    {
        await _service.ActualizarAsync(id, dto);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = "Empresa actualizada exitosamente"
        });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarEmpresaAsync(int id)
    {
        await _service.EliminarAsync(id);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = "Empresa eliminada exitosamente"
        });
    }
    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarEmpresaAsync(int id)
    {
        await _service.DesactivarAsync(id);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = "Empresa desactivada exitosamente"
        });
    }
}

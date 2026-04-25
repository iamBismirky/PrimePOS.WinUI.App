using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Cliente;

namespace PrimePOS.API.Controllers;

[Authorize]
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
        var clientes = await _service.ObtenerTodosAsync();

        return Ok(new ApiResponse<List<ClienteDto>>
        {
            Success = true,
            Data = clientes
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerClientePorIdAsync(int id)
    {
        var cliente = await _service.ObtenerPorIdAsync(id);

        return Ok(new ApiResponse<ClienteDto>
        {
            Success = true,
            Data = cliente
        });
    }

    [HttpPost]
    public async Task<IActionResult> CrearClienteAsync([FromBody] CrearClienteDto dto)
    {
        await _service.CrearClienteAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cliente creado correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarClienteAsync(int id, [FromBody] ActualizarClienteDto dto)
    {
        dto.ClienteId = id;

        await _service.ActualizarClienteAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cliente actualizado correctamente"
        });
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> DesactivarClienteAsync(int id)
    {
        await _service.DesactivarClienteAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cliente desactivado correctamente"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarClienteAsync(int id)
    {
        await _service.EliminarClienteAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cliente eliminado correctamente"
        });
    }
}
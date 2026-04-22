using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Usuario;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public AuthController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AutenticarUsuarioDto dto)
    {
        var result = await _usuarioService.AutenticarUsuarioAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario autenticado correctamente",
        });
    }
}
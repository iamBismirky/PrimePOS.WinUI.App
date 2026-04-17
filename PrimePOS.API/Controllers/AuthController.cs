using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
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
    public async Task<IActionResult> Login(AutenticarUsuarioDto dto)
    {
        var result = await _usuarioService.AutenticarUsuarioAsync(dto);

        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
}
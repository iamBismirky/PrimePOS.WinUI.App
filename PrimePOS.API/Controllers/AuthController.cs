using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Exceptions;
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

        try
        {
            var result = await _usuarioService.AutenticarUsuarioAsync(dto);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
        catch (BusinessException ex)
        {

            return BadRequest(new { message = ex.Message, code = ex.Code });
        }
    }
}
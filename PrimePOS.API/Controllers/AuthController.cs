using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimePOS.API.Security;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Usuario;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IAuthService usuarioService, JwtHelper jwtHelper)
    {
        _authService = usuarioService;
        _jwtHelper = jwtHelper;
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _authService.AutenticarUsuarioAsync(dto);

        var token = _jwtHelper.GenerarToken(
            usuario.UsuarioId,
            usuario.Username,
            usuario.UsuarioNombre,
            usuario.RolNombre ?? ""
        );

        var result = new AppSesionUsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            UsuarioNombre = $"{usuario.UsuarioNombre}",
            Username = usuario.Username,
            RolId = usuario.RolId,
            RolNombre = usuario.RolNombre ?? "",
            Token = token
        };

        return Ok(new ApiResponse<AppSesionUsuarioDto>
        {
            Success = true,
            Message = "Usuario autenticado correctamente",
            Data = result,
        });
    }
}
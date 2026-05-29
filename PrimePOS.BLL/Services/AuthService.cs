using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Security;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.DAL.Interfaces;

namespace PrimePOS.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        public AuthService(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public async Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new BusinessException("Usuario y contraseña obligatorios.", StatusCodes.Status400BadRequest);

            var usuario = await _usuarioRepo.ObtenerPorUsernameAsync(dto.Username);

            if (usuario == null)
                throw new BusinessException("Usuario o contraseña incorrectos.", StatusCodes.Status401Unauthorized);

            if (!usuario.Estado)
                throw new BusinessException("Usuario inactivo.", StatusCodes.Status403Forbidden);

            if (!PasswordService.Verify(dto.Password, usuario.Password))
                throw new BusinessException("Usuario o contraseña incorrectos.", StatusCodes.Status401Unauthorized);



            return new AppSesionUsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                UsuarioNombre = $"{usuario.Nombre} {usuario.Apellidos}",
                Username = usuario.Username,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? "",

            };
        }
    }
}

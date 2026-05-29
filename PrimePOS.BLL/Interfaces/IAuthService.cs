using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(LoginDto dto);
    }
}
using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task ActualizarUsuarioAsync(ActualizarUsuarioDto dto);
        Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(AutenticarUsuarioDto dto);
        Task CambiarPasswordAsync(int usuarioId, CambiarPasswordDto dto);
        Task CambiarEstadoAsync(int id, bool nuevoEstado);
        Task CrearUsuarioAsync(CrearUsuarioDto dto);
        Task DesactivarUsuarioAsync(int usuarioId);
        Task EliminarUsuarioAsync(int usuarioId);
        Task<List<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id);
    }
}
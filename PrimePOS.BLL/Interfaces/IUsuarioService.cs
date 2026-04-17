using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task ActualizarUsuarioAsync(ActualizarUsuarioDto dto);
        Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(AutenticarUsuarioDto dto);
        Task CambiarContraseñaAsync(CambiarContraseñaDto dto);
        Task CambiarEstadoAsync(int id, bool nuevoEstado);
        Task CrearUsuarioAsync(CrearUsuarioDto dto);
        Task<bool> DesactivarUsuarioAsync(int usuarioId);
        Task<bool> EliminarUsuarioAsync(int usuarioId);
        Task<List<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id);
    }
}
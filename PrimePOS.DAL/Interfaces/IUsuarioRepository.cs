using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IUsuarioRepository
    {
        void Actualizar(Usuario usuario);
        void Crear(Usuario usuario);
        void Eliminar(Usuario usuario);
        Task<bool> ExisteUsernameAsync(string username, int? excluirId = null);
        Task GuardarCambiosAsync();
        Task<Usuario?> ObtenerPorId(int id);
        Task<Usuario?> ObtenerPorUsernameAsync(string username);
        Task<List<Usuario>> ObtenerTodosAsync();
    }
}
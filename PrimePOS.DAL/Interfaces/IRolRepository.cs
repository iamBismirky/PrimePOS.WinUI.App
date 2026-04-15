using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IRolRepository
    {
        Task Actualizar(Rol rol);
        Task Crear(Rol rol);
        Task Eliminar(Rol rol);
        Task<Rol> ExisteRolAsync(string nombre);
        Task GuardarCambiosAsync();
        Task<List<Rol>> ListarRolesAsync();
        Task<Rol?> ObtenerPorIdAsync(int rolId);
    }
}
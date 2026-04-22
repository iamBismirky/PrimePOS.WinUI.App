using PrimePOS.Contracts.DTOs.Rol;

namespace PrimePOS.BLL.Interfaces
{
    public interface IRolService
    {
        Task ActualizarRolAsync(ActualizarRolDto dto);
        Task CrearRolAsync(CrearRolDto dto);

        Task EliminarRolAsync(int rolId);
        Task DesactivarRolAsync(int rolId);
        Task<List<RolDto>> ListarRolesAsync();
        Task<RolDto?> ObtenerRolPorIdAsync(int id);
    }
}
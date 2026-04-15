using PrimePOS.Contracts.DTOs.Rol;

namespace PrimePOS.BLL.Interfaces
{
    public interface IRolService
    {
        Task<bool> ActualizarRolAsync(ActualizarRolDto dto);
        Task CrearRolAsync(CrearRolDto dto);

        Task<bool> EliminarRolAsync(EliminarRolDto dto);
        Task DesactivarRolAsync(int rolId);
        Task<List<ListaRolesDto>> ListarRolesAsync();
        Task<RolDto?> ObtenerRolPorIdAsync(int id);
    }
}
using PrimePOS.BLL.DTOs.Rol;

namespace PrimePOS.BLL.Interfaces
{
    public interface IRolService
    {
        Task<bool> ActualizarRolAsync(ActualizarRolDto dto);
        Task CrearRolAsync(CrearRolDto dto);
        Task<bool> DesactivarRolAsync(RolDto dto);
        Task<bool> EliminarRolAsync(EliminarRolDto dto);
        Task<List<ListaRolesDto>> ListarRolesAsync();
        Task<RolDto?> ObtenerRolPorIdAsync(int id);
    }
}
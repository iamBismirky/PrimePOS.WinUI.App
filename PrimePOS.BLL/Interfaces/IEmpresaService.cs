using Microsoft.AspNetCore.Http;
using PrimePOS.Contracts.DTOs.Empresa;

namespace PrimePOS.BLL.Interfaces
{
    public interface IEmpresaService
    {
        Task ActualizarAsync(int id, ActualizarEmpresaDto dto);
        Task<int> CrearAsync(CrearEmpresaDto dto);
        Task DesactivarAsync(int empresaId);
        Task EliminarAsync(int empresaId);
        Task<EmpresaDto?> ObtenerPorIdAsync(int empresaId);
        Task<List<EmpresaDto>> ObtenerTodosAsync();
        Task SubirLogoAsync(int empresaId, IFormFile logo);
    }
}
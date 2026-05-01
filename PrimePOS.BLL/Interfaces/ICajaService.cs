using PrimePOS.Contracts.DTOs.Caja;

namespace PrimePOS.BLL.Interfaces
{
    public interface ICajaService
    {
        Task ActualizarCajaAsync(CajaDto dto);
        Task CrearCajaAsync(CajaDto dto);
        Task DesactivarCajaAsync(int cajaId);
        Task EliminarCajaAsync(int cajaId);
        Task<List<CajaDto>> ListarCajasAsync();
        Task<CajaDto?> ObtenerCajaPorIdAsync(int id);
    }
}
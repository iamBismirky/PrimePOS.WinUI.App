using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Interfaces
{
    public interface IMetodoPagoRepository
    {
        Task<List<MetodoPago>> ListarMetodosPagosAsync();
        Task<MetodoPago?> ObtenerPorIdAsync(int id);
    }
}
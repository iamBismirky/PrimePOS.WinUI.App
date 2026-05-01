using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IFacturaRepository
    {
        Task<int> ContarFacturasAsync();
        Task CrearAsync(Factura factura);
        Task GuardarCambiosAsync();
        Task<Factura?> ObtenerPorIdAsync(int id);
        Task<List<Factura>> ObtenerTodosAsync();
    }
}
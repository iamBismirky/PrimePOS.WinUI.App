using PrimePOS.ENTITIES.Models.Facturacion;

namespace PrimePOS.DAL.Interfaces
{
    public interface IFacturaRepository
    {
        Task<int> ContarFacturasAsync();
        void Crear(Factura factura);
        Task GuardarCambiosAsync();
        Task<Factura?> ObtenerPorIdAsync(int id);
        Task<List<Factura>> ObtenerTodosAsync();
    }
}
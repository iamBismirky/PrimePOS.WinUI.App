using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IVentaRepository
    {
        void Actualizar(Venta venta);
        void Anular(Venta venta);
        void Crear(Venta venta);
        Task GuardarCambiosAsync();
        List<Venta> ListarVentas();
        Task<List<Venta>> ObtenerPorFechaAsync(DateTime fecha);
        Task<Venta?> ObtenerPorId(int id);
        Task<List<Venta>> ObtenerPorTurnoAsync(int turnoId);
        Task<decimal> ObtenerTotalPorMetodoPagoAsync(int turnoId, int metodoPagoId);
    }
}
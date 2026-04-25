using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Interfaces
{
    public interface IFacturaService
    {
        Task AnularFactura(int facturaId);
        Task<Factura> GenerarFacturaDesdeVenta(int ventaId);
        FacturaDto MapearFactura(Factura factura);
    }
}
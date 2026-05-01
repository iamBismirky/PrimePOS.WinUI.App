using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Interfaces
{
    public interface IFacturaService
    {
        Task<(Factura factura, string pdfUrl)> GenerarFacturaDesdeVenta(int ventaId);

        Task AnularFactura(int facturaId);
        FacturaDto MapearFactura(Factura factura);
    }
}
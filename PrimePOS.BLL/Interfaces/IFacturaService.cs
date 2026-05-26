using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.ENTITIES.Models.Facturacion;

namespace PrimePOS.BLL.Interfaces
{
    public interface IFacturaService
    {


        Task AnularFactura(int facturaId);
        FacturaDto MapearFactura(Factura factura);
        Task<Factura> CrearFactura(int ventaId);
        Task<FacturaGeneradaDto> GenerarFacturaDesdeVenta(int ventaId);
    }
}
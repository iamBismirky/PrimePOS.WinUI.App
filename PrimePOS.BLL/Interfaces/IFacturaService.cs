using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.ENTITIES.Models.Facturacion;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.BLL.Interfaces
{
    public interface IFacturaService
    {


        Task AnularFactura(int facturaId);
        FacturaDto MapearFactura(Factura factura);
        Task<Factura> CrearFactura(Venta venta);
        Task<List<FacturaListadoDto>> ObtenerTodosAsync();
    }
}
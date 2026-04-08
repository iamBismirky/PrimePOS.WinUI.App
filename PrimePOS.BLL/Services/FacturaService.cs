using PrimePOS.BLL.DTOs.Factura;
using PrimePOS.BLL.DTOs.FacturaDetalle;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class FacturaService
    {
        private readonly FacturaRepository _facturaRepository;
        private readonly VentaRepository _ventaRepository;

        public FacturaService(FacturaRepository repo, VentaRepository ventaRepository)
        {
            _facturaRepository = repo;
            _ventaRepository = ventaRepository;
        }

        public async Task<Factura> GenerarFacturaDesdeVenta(int ventaId)
        {
            var venta = await _ventaRepository.ObtenerPorId(ventaId);

            if (venta == null) throw new Exception("Venta no encontrada");

            var factura = new Factura
            {
                Numero = "",
                Fecha = DateTime.Now,
                VentaId = venta.VentaId,
                ClienteId = venta.ClienteId,
                ClienteNombre = venta.ClienteNombre,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = venta.UsuarioNombre,
                Subtotal = venta.Subtotal,
                Descuento = venta.Descuento,
                Impuesto = venta.Impuesto,
                Total = venta.Total,
                MetodoPago = venta.MetodoPago?.Nombre ?? "",
                Efectivo = venta.Efectivo,
                Cambio = venta.Cambio,

            };

            foreach (var item in venta.Detalles)
            {
                factura.Detalles.Add(new FacturaDetalle
                {
                    ProductoNombre = item.Producto?.Nombre ?? "",
                    ProductoId = item.ProductoId,
                    Precio = item.PrecioUnitario,
                    Cantidad = item.Cantidad,
                    Total = item.Total
                });
            }

            await _facturaRepository.CrearAsync(factura);
            await _facturaRepository.GuardarCambiosAsync();

            factura.Numero = $"F-{factura.FacturaId:D6}";
            await _facturaRepository.GuardarCambiosAsync();

            return factura;
        }
        public async Task AnularFactura(int facturaId)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(facturaId);

            if (factura == null)
                return;

            factura.Estado = "Anulada";

            await _facturaRepository.GuardarCambiosAsync();
        }
        public FacturaDto MapearFactura(Factura factura)
        {
            return new FacturaDto
            {
                Numero = factura.Numero,
                Fecha = factura.Fecha,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Total = factura.Total,
                MetodoPago = factura.MetodoPago,
                Turno = factura.Venta?.Turno?.NumeroTurno.ToString() ?? "",
                ClienteNombre = factura.Venta?.Cliente?.Nombre ?? "",
                UsuarioNombre = factura.Usuario?.Nombre ?? "",
                Efectivo = factura.Efectivo,
                Cambio = factura.Cambio,

                Detalles = factura.Detalles.Select(d => new FacturaDetalleDto
                {
                    Nombre = d.ProductoNombre,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Total = d.Total

                }).ToList()
            };
        }

    }
}

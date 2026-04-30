using Microsoft.Extensions.Configuration;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Reportes;
using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.Contracts.DTOs.FacturaDetalle;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;
using QuestPDF.Fluent;

namespace PrimePOS.BLL.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IConfiguration _config;

        public FacturaService(IFacturaRepository repo, IVentaRepository ventaRepository, IConfiguration config)
        {
            _facturaRepository = repo;
            _ventaRepository = ventaRepository;
            _config = config;
        }


        public async Task<(Factura factura, string pdfUrl)> GenerarFacturaDesdeVenta(int ventaId)
        {
            var venta = await _ventaRepository.ObtenerPorId(ventaId);

            if (venta == null)
                throw new Exception("Venta no encontrada");

            var factura = new Factura
            {
                Detalles = new List<FacturaDetalle>(),
                Fecha = DateTime.Now,
                VentaId = venta.VentaId,

                ClienteId = venta.ClienteId,
                ClienteNombre = venta.ClienteNombre,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = venta.UsuarioNombre,

                Subtotal = venta.Subtotal,
                Impuesto = venta.Impuesto,
                Descuento = venta.Descuento,
                Total = venta.Total,

                MetodoPago = venta.MetodoPago?.Nombre ?? "",
                Efectivo = venta.Efectivo,
                Cambio = venta.Cambio
            };

            foreach (var item in venta.Detalles)
            {
                factura.Detalles.Add(new FacturaDetalle
                {
                    ProductoId = item.ProductoId,
                    ProductoNombre = item.ProductoNombre,
                    Cantidad = item.Cantidad,
                    Precio = item.PrecioUnitario,
                    Total = item.Total
                });
            }

            await _facturaRepository.CrearAsync(factura);
            await _facturaRepository.GuardarCambiosAsync();

            factura.Numero = $"F-{factura.FacturaId:D6}";
            await _facturaRepository.GuardarCambiosAsync();

            // 🔥 PDF
            var url = GenerarPdf(factura);

            return (factura, url);
        }
        private string GenerarPdf(Factura factura)
        {
            var baseUrl = _config["App:BaseUrl"]; // ej: https://localhost:5001

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "facturas");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"factura-{factura.Numero}.pdf";
            var path = Path.Combine(folder, fileName);

            var document = new Factura80mmDocument(factura);
            document.GeneratePdf(path);

            return $"{baseUrl}/facturas/{fileName}";
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

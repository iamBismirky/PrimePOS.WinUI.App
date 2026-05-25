using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Reportes;
using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.Contracts.DTOs.FacturaDetalle;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Factura;
using PrimePOS.ENTITIES.Models.Ventas;
using QuestPDF.Fluent;

namespace PrimePOS.BLL.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;


        public FacturaService(IFacturaRepository repo,
            IVentaRepository ventaRepository,
            IConfiguration config,
            IHostEnvironment env)
        {
            _facturaRepository = repo;
            _ventaRepository = ventaRepository;
            _config = config;
            _env = env;
        }
        public async Task<FacturaGeneradaDto> GenerarFacturaDesdeVenta(int ventaId)
        {
            var factura = await CrearFactura(ventaId);

            var facturaDto = MapearFactura(factura);

            var pdfUrl = await GenerarPdf(facturaDto);

            return new FacturaGeneradaDto
            {
                Factura = facturaDto,
                PdfUrl = pdfUrl
            };
        }
        public async Task<Factura> CrearFactura(int ventaId)
        {
            var venta = await _ventaRepository.ObtenerPorId(ventaId);

            if (venta == null)
                throw new BusinessException("Venta no encontrada", 404);

            var factura = new Factura
            {
                Fecha = DateTime.Now,
                VentaId = venta.VentaId,
                ClienteId = venta.ClienteId ?? 0,
                ClienteNombre = venta.ClienteNombre,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = venta.UsuarioNombre,

                Subtotal = venta.Subtotal,
                Impuesto = venta.Impuesto,
                Descuento = venta.Descuento,
                Total = venta.Total,

                MetodoPago = venta.MetodoPago?.Nombre ?? "",
                Efectivo = venta.MontoRecibido,
                Cambio = venta.Cambio,

                Detalles = new List<FacturaDetalle>()
            };

            foreach (var item in venta.Detalles)
            {
                factura.Detalles.Add(new FacturaDetalle
                {
                    ProductoId = item.ProductoId,
                    ProductoNombre = item.Producto?.Nombre ?? "",
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Total = item.Total
                });
            }

            await _facturaRepository.CrearAsync(factura);
            await _facturaRepository.GuardarCambiosAsync();

            factura.Numero = $"F-{factura.FacturaId:D6}";
            await _facturaRepository.GuardarCambiosAsync();

            return factura;
        }
        private async Task<string> GenerarPdf(FacturaDto factura)
        {
            var folder = Path.Combine(_env.ContentRootPath, "wwwroot", "facturas");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"factura-{factura.Numero}.pdf";

            var path = Path.Combine(folder, fileName);

            var document = new Factura80mmDocument(factura);

            document.GeneratePdf(path);

            var baseUrl = _config["App:BaseUrl"];

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
                MetodoPago = factura.MetodoPago?.ToString(),
                Turno = factura.Venta?.Turno?.NumeroTurno.ToString() ?? "",
                ClienteNombre = factura.ClienteNombre ?? "",
                UsuarioNombre = factura.UsuarioNombre ?? "",
                Efectivo = factura.Efectivo,
                Cambio = factura.Cambio,

                Detalles = factura.Detalles.Select(d => new FacturaDetalleDto
                {
                    Nombre = d.ProductoNombre,
                    Cantidad = d.Cantidad,
                    Precio = d.PrecioUnitario,
                    Total = d.Total

                }).ToList()
            };
        }

    }
}

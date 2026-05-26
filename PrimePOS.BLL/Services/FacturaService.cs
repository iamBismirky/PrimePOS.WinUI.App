using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Reportes;
using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Facturacion;
using QuestPDF.Fluent;

namespace PrimePOS.BLL.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly ITurnoRepository _turnoRepository;
        private readonly ICatalogRepository _catalogRepo;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;


        public FacturaService(IFacturaRepository repo,
            IVentaRepository ventaRepository,
            IConfiguration config,
            IHostEnvironment env,
            ITurnoRepository turnoRepository,
            ICatalogRepository catalogRepo)
        {
            _facturaRepository = repo;
            _ventaRepository = ventaRepository;
            _config = config;
            _env = env;
            _turnoRepository = turnoRepository;
            _catalogRepo = catalogRepo;
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
            var turno = await _turnoRepository.ObtenerPorIdAsync(venta!.TurnoId);
            var metodoPago = await _catalogRepo.ObtenerMetodoPagoAsync(venta.MetodoPagoId);
            var tipoVenta = await _catalogRepo.ObtenerTipoVentaAsync(venta.TipoVentaId);

            if (venta == null)
                throw new BusinessException("Venta no encontrada", 404);

            var factura = new Factura
            {
                Fecha = venta.FechaRegistro,
                VentaId = venta.VentaId,
                TipoFactura = tipoVenta?.Nombre ?? "",
                ClienteId = venta.ClienteId ?? 0,
                ClienteNombre = venta.ClienteNombre,
                UsuarioId = venta.UsuarioId,
                UsuarioNombre = venta.UsuarioNombre,
                TurnoId = venta.TurnoId,
                NumeroTurno = turno!.NumeroTurno.ToString(),
                Subtotal = venta.Subtotal,
                Impuesto = venta.Impuesto,
                Descuento = venta.Descuento,
                Total = venta.Total,
                MetodoPagoId = venta.MetodoPagoId,
                MetodoPago = metodoPago?.Nombre ?? "",
                Efectivo = venta.MontoRecibido,
                Cambio = venta.Cambio,
                Estado = venta.EstadoVenta?.Estado ?? "",
                BalancePendiente = venta.BalancePendiente,
                Detalles = new List<FacturaDetalle>()
            };

            foreach (var item in venta.Detalles)
            {
                factura.Detalles.Add(new FacturaDetalle
                {
                    ProductoId = item.ProductoId,
                    Nombre = item.Nombre,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    AplicaItbis = item.AplicaItbis,
                    Itbis = item.Itbis,
                    Subtotal = item.Subtotal,
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
                TipoFactura = factura.TipoFactura,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Total = factura.Total,
                MetodoPago = factura.MetodoPago?.ToString(),
                Turno = factura.Venta?.Turno?.NumeroTurno.ToString() ?? "",
                ClienteNombre = factura.ClienteNombre ?? "",
                UsuarioNombre = factura.UsuarioNombre ?? "",
                Efectivo = factura.Efectivo,
                Cambio = factura.Cambio,
                Descuento = factura.Descuento,
                BalancePendiente = factura.BalancePendiente,

                Detalles = factura.Detalles.Select(d => new FacturaDetalleDto
                {
                    Nombre = d.Nombre,
                    Cantidad = d.Cantidad,
                    Precio = d.PrecioUnitario,
                    Total = d.Total

                }).ToList()
            };
        }

    }
}

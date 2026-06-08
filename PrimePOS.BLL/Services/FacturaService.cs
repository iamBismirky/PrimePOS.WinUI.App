using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Facturacion;
using PrimePOS.ENTITIES.Models.Ventas;

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
        private readonly IPdfService _pdfService;
        private readonly IUnitOfWork _unitOfWork;


        public FacturaService(IFacturaRepository repo,
            IVentaRepository ventaRepository,
            IConfiguration config,
            IHostEnvironment env,
            ITurnoRepository turnoRepository,
            ICatalogRepository catalogRepo,
            IPdfService pdfService,
            IUnitOfWork unitOfWork)
        {
            _facturaRepository = repo;
            _ventaRepository = ventaRepository;
            _config = config;
            _env = env;
            _turnoRepository = turnoRepository;
            _catalogRepo = catalogRepo;
            _pdfService = pdfService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Factura> CrearFactura(Venta venta)
        {
            if (venta == null)
                throw new BusinessException(
                    "Venta no encontrada",
                    StatusCodes.Status404NotFound);

            var turno =
                await _turnoRepository.ObtenerPorIdAsync(venta.TurnoId);

            var metodoPago =
                await _catalogRepo.ObtenerMetodoPagoAsync(
                    venta.MetodoPagoId);

            var tipoVenta =
                await _catalogRepo.ObtenerTipoVentaAsync(
                    venta.TipoVentaId);

            var estadoVenta =
                await _catalogRepo.ObtenerEstadoVentaAsync(
                    venta.EstadoVentaId);

            var factura = new Factura
            {
                Fecha = venta.FechaRegistro,

                VentaId = venta.VentaId,

                Numero = $"F-{venta.VentaId:D6}",

                TipoFactura = tipoVenta?.Nombre ?? "",

                ClienteId = venta.ClienteId ?? 0,
                ClienteNombre = venta.ClienteNombre,

                UsuarioId = venta.UsuarioId,
                UsuarioNombre = venta.UsuarioNombre,

                TurnoId = venta.TurnoId,
                NumeroTurno = turno?.NumeroTurno ?? 0,

                CajaId = venta.CajaId,
                CajaNombre = venta.CajaNombre,

                TipoPrecioId = venta.TipoPrecioId,
                TipoPrecioNombre = venta.TipoPrecioNombre,

                Subtotal = venta.Subtotal,
                Impuesto = venta.Impuesto,
                Descuento = venta.Descuento,
                Total = venta.Total,

                MetodoPagoId = venta.MetodoPagoId,
                MetodoPago = metodoPago?.Nombre ?? "",

                Efectivo = venta.MontoRecibido,
                Cambio = venta.Cambio,

                EstadoFacturaId = venta.EstadoVentaId,
                EstadoFacturaNombre = estadoVenta?.Codigo ?? "",

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

            _facturaRepository.Crear(factura);

            await _unitOfWork.SaveChangesAsync();

            return factura;
        }



        public async Task AnularFactura(int facturaId)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(facturaId);

            if (factura == null)
                throw new BusinessException("Factura no encontrada", StatusCodes.Status404NotFound);

            factura.EstadoFacturaNombre = "Anulada";

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
                EstadoFacturaNombre = factura.EstadoFacturaNombre,
                CajaNombre = factura.CajaNombre,

                Detalles = factura.Detalles.Select(d => new FacturaDetalleDto
                {
                    Nombre = d.Nombre,
                    Cantidad = d.Cantidad,
                    Precio = d.PrecioUnitario,
                    Total = d.Total

                }).ToList()
            };
        }

        public async Task<List<FacturaListadoDto>> ObtenerTodosAsync()
        {
            var productos = await _facturaRepository.ObtenerTodosAsync();
            if (productos == null)
                throw new BusinessException("No se encontraron facturas", StatusCodes.Status404NotFound);

            return productos.Select(p => new FacturaListadoDto
            {
                FacturaId = p.FacturaId,
                NumeroFactura = p.Numero,
                ClienteNombre = p.ClienteNombre ?? "",
                UsuarioNombre = p.UsuarioNombre ?? "",
                Fecha = p.Fecha,
                TipoVenta = p.TipoFactura,
                Estado = p.EstadoFacturaNombre,
                Total = p.Total
            }).ToList();
        }
    }



}

using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.BLL.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IFacturaService _facturaService;
    private readonly ICatalogRepository _catalogRepository;
    private readonly IUnitOfWork _unitOfWork;
    public VentaService(IVentaRepository ventaRepository,
        IProductoRepository productoRepository,
        IFacturaService facturaService, ICatalogRepository catalogRepository, IUnitOfWork unitOfWork)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _facturaService = facturaService;
        _catalogRepository = catalogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> ObtenerVentasPorTurnoAsync(int turnoId)
    {
        return await _ventaRepository
            .ObtenerTotalVentasPorTurnoAsync(turnoId);
    }

    public async Task<List<VentaDto>> ObtenerVentasDelDiaAsync()
    {
        var ventas = await _ventaRepository.ObtenerPorFechaAsync(DateTime.Today);

        return ventas.Select(v => new VentaDto
        {
            VentaId = v.VentaId,
            FechaRegistro = v.FechaRegistro,
            ClienteNombre = v.ClienteNombre,
            MetodoPagoId = v.MetodoPagoId,
            Total = v.Total
        }).ToList();
    }


    public async Task<int> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto)
    {
        if (dto.TurnoId <= 0)
        {
            throw new BusinessException(
                "Turno inválido.",
                StatusCodes.Status400BadRequest);
        }

        if (dto.CajaId <= 0)
        {
            throw new BusinessException(
                "Caja inválida.",
                StatusCodes.Status400BadRequest);
        }

        if (!dto.Items.Any())
        {
            throw new BusinessException(
                "La venta no tiene productos.",
                StatusCodes.Status400BadRequest);
        }

        var tipoVenta =
            await _catalogRepository
                .ObtenerTipoVentaAsync(dto.TipoVentaId);

        if (tipoVenta is null)
        {
            throw new BusinessException(
                "Tipo de venta inválido.",
                StatusCodes.Status400BadRequest);
        }

        var tipoPrecio =
            await _catalogRepository
                .ObtenerTipoPrecioAsync(dto.TipoPrecioId);

        if (tipoPrecio is null)
        {
            throw new BusinessException(
                "Tipo de precio inválido.",
                StatusCodes.Status400BadRequest);
        }

        var metodoPago =
            await _catalogRepository
                .ObtenerMetodoPagoAsync(dto.MetodoPagoId);

        if (metodoPago is null)
        {
            throw new BusinessException(
                "Método de pago inválido.",
                StatusCodes.Status400BadRequest);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var productoIds = dto.Items
                .Select(x => x.ProductoId)
                .Distinct()
                .ToList();

            var productos = await _productoRepository
                .ObtenerPorIdsAsync(productoIds);

            var productosDict = productos
                .ToDictionary(x => x.ProductoId);

            var venta = new Venta
            {
                TurnoId = dto.TurnoId,
                CajaId = dto.CajaId,

                UsuarioId = userId,
                UsuarioNombre = nombre,

                ClienteId = dto.ClienteId,
                ClienteNombre = dto.ClienteNombre,

                MetodoPagoId = metodoPago.MetodoPagoId,

                TipoVentaId = tipoVenta.TipoVentaId,
                TipoPrecioId = tipoPrecio.TipoPrecioId,

                FechaRegistro = DateTime.Now,

                MontoRecibido = dto.MontoRecibido
            };

            foreach (var item in dto.Items)
            {
                if (!productosDict.TryGetValue(
                    item.ProductoId,
                    out var producto))
                {
                    throw new BusinessException(
                        $"Producto inválido ID {item.ProductoId}.",
                        StatusCodes.Status404NotFound);
                }

                if (producto.Existencia < item.Cantidad)
                {
                    throw new BusinessException(
                        $"Stock insuficiente para {producto.Nombre}.",
                        StatusCodes.Status400BadRequest);
                }

                var precioUnitario =
                    ObtenerPrecioProducto(
                        producto,
                        tipoPrecio);

                var subtotal =
                    precioUnitario * item.Cantidad;

                decimal impuesto = 0;

                if (producto.AplicaItbis)
                {
                    impuesto =
                        subtotal *
                        (producto.ItbisPorcentaje / 100m);
                }

                var total = subtotal + impuesto;

                producto.Existencia -= item.Cantidad;

                var detalle = new VentaDetalle
                {
                    ProductoId = producto.ProductoId,

                    Nombre = producto.Nombre,

                    Codigo = producto.Codigo,

                    Cantidad = item.Cantidad,

                    PrecioUnitario = precioUnitario,

                    Subtotal = subtotal,

                    AplicaItbis = producto.AplicaItbis,

                    Itbis = impuesto,

                    Total = total
                };

                venta.Detalles.Add(detalle);
            }

            RecalcularTotales(
                venta,
                tipoVenta);

            ValidarVenta(
                venta,
                tipoVenta);

            AsignarEstadoVenta(venta);

            _ventaRepository.Crear(venta);

            await _ventaRepository.GuardarCambiosAsync();

            await _unitOfWork.CommitAsync();

            //var (factura, pdfUrl) =
            //    await _facturaService
            //        .GenerarFacturaDesdeVenta(
            //            venta.VentaId);

            return venta.VentaId;
            //return new VentaResponseDto
            //{
            //    VentaId = venta.VentaId,

            //    NumeroFactura = factura.Numero,

            //    FileName =
            //        $"factura-{factura.Numero}.pdf",

            //    Fecha = venta.FechaRegistro,

            //    Total = venta.Total
            //};
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }







    private static decimal ObtenerPrecioProducto(
    Producto producto,
    TipoPrecio tipoPrecio)
    {
        return tipoPrecio.Codigo switch
        {
            "MAYORISTA" => producto.PrecioMayorista,

            _ => producto.PrecioMinorista
        };
    }
    private static void RecalcularTotales(Venta venta, TipoVenta tipoVenta)

    {
        venta.Subtotal =
            venta.Detalles.Sum(x => x.Subtotal);

        venta.Impuesto =
            venta.Detalles.Sum(x => x.Itbis);

        venta.Total = venta.Subtotal + venta.Impuesto - venta.Descuento;


        if (tipoVenta.Codigo == "CONTADO")
        {
            venta.Cambio =
                Math.Max(
                    0,
                    venta.MontoRecibido - venta.Total);

            venta.BalancePendiente = 0;
        }
        else
        {
            venta.Cambio = 0;

            venta.BalancePendiente =
                Math.Max(
                    0,
                    venta.Total - venta.MontoRecibido);
        }
    }

    private static void ValidarVenta(
        Venta venta,
        TipoVenta tipoVenta)
    {
        if (tipoVenta.Codigo == "CONTADO")
        {
            if (venta.MontoRecibido < venta.Total)
            {
                throw new BusinessException(
                    "El monto recibido es insuficiente.",
                    StatusCodes.Status400BadRequest);
            }
        }

        if (tipoVenta.Codigo == "CREDITO")
        {
            if (!venta.ClienteId.HasValue)
            {
                throw new BusinessException(
                    "La venta a crédito requiere cliente.",
                    StatusCodes.Status400BadRequest);
            }
        }
    }

    private static void AsignarEstadoVenta(Venta venta)
    {
        if (venta.BalancePendiente <= 0)
        {
            venta.EstadoVentaId = 1;
            return;
        }

        if (venta.MontoRecibido <= 0)
        {
            venta.EstadoVentaId = 2;
            return;
        }

        venta.EstadoVentaId = 3;
    }
}

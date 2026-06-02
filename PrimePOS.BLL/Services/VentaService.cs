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
    private readonly ICatalogRepository _catalogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFacturaService _facturaService;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IPdfService _pdfService;
    private readonly ITurnoRepository _turnoRepository;
    private readonly ICajaRepository _cajaRepository;
    private readonly IClienteRepository _clienteRepository;
    public VentaService(IVentaRepository ventaRepository,
        IProductoRepository productoRepository,
        ICatalogRepository catalogRepository,
        IUnitOfWork unitOfWork,
        IFacturaService facturaService,
        IFacturaRepository facturaRepository,
        IPdfService pdfService,
        ITurnoRepository turnoRepository,
        ICajaRepository cajaRepository,
        IClienteRepository clienteRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _catalogRepository = catalogRepository;
        _unitOfWork = unitOfWork;
        _facturaService = facturaService;
        _facturaRepository = facturaRepository;
        _pdfService = pdfService;
        _turnoRepository = turnoRepository;
        _cajaRepository = cajaRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<VentaConFacturaDto> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto)
    {
        var turno = await _turnoRepository.ObtenerPorIdAsync(dto.TurnoId);
        if (turno is null)
            throw new BusinessException("Turno inválido.", StatusCodes.Status400BadRequest);

        var caja = await _cajaRepository.ObtenerCajaPorIdAsync(dto.CajaId);
        if (caja is null)
            throw new BusinessException("Caja inválida.", StatusCodes.Status400BadRequest);


        if (!dto.Items.Any())
            throw new BusinessException("La venta no tiene productos.", StatusCodes.Status400BadRequest);


        var tipoVenta = await _catalogRepository.ObtenerTipoVentaAsync(dto.TipoVentaId);

        if (tipoVenta is null)
            throw new BusinessException("Tipo de venta inválido.", StatusCodes.Status400BadRequest);


        var tipoPrecio = await _catalogRepository.ObtenerTipoPrecioAsync(dto.TipoPrecioId);

        if (tipoPrecio is null)
            throw new BusinessException("Tipo de precio inválido.", StatusCodes.Status400BadRequest);


        var metodoPago = await _catalogRepository.ObtenerMetodoPagoAsync(dto.MetodoPagoId);

        if (metodoPago is null)
            throw new BusinessException("Método de pago inválido.", StatusCodes.Status400BadRequest);

        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);
        if (cliente is null)
            throw new BusinessException("Cliente inválido.", StatusCodes.Status400BadRequest);

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
                TurnoId = turno.TurnoId,
                NumeroTurno = turno.NumeroTurno,
                CajaId = caja.CajaId,
                CajaNombre = caja.Nombre,
                UsuarioId = userId,
                UsuarioNombre = nombre,

                ClienteId = cliente.ClienteId,
                ClienteNombre = cliente.Nombre,

                MetodoPagoId = metodoPago.MetodoPagoId,
                MetodoPagoNombre = metodoPago.Nombre,
                TipoVentaId = tipoVenta.TipoVentaId,
                TipoVentaNombre = tipoVenta.Nombre,
                TipoPrecioId = tipoPrecio.TipoPrecioId,
                TipoPrecioNombre = tipoPrecio.Nombre,

                FechaRegistro = DateTime.Now,
                MontoRecibido = dto.MontoRecibido,

                Detalles = new List<VentaDetalle>()
            };

            foreach (var item in dto.Items)
            {
                if (!productosDict.TryGetValue(item.ProductoId, out var producto))
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

                var precioUnitario = ObtenerPrecioProducto(producto, tipoPrecio);

                var subtotal = precioUnitario * item.Cantidad;

                decimal impuesto = 0;

                if (producto.AplicaItbis)
                {
                    impuesto = subtotal * (producto.ItbisPorcentaje / 100m);
                }

                var total = subtotal + impuesto;

                producto.Existencia -= item.Cantidad;

                venta.Detalles.Add(new VentaDetalle
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
                });
            }

            RecalcularTotales(venta, tipoVenta);
            ValidarVenta(venta, tipoVenta);
            AsignarEstadoVenta(venta);

            _ventaRepository.Crear(venta);
            await _unitOfWork.SaveChangesAsync();


            var factura = await _facturaService.CrearFactura(venta);


            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();


            // =========================
            // 🔥 ORQUESTACIÓN
            // =========================


            var facturaDto = _facturaService.MapearFactura(factura);

            var pdfFileName = _pdfService.GenerateFacturaPdf(facturaDto);

            var pdfUrl = _pdfService.BuildFacturaUrl(pdfFileName);

            return new VentaConFacturaDto
            {
                VentaId = venta.VentaId,
                FacturaId = factura.FacturaId,
                NumeroFactura = factura.Numero,
                PdfUrl = pdfUrl,
                FileName = pdfFileName
            };
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
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
            venta.EstadoVentaNombre = "Pagada";
            return;
        }

        if (venta.MontoRecibido <= 0)
        {
            venta.EstadoVentaId = 2;
            venta.EstadoVentaNombre = "Pendiente";
            return;
        }

        venta.EstadoVentaId = 3;
        venta.EstadoVentaNombre = "Parcial";
    }

    //public async Task<List<ProductoVentaDto>> BuscarProductosAsync(string texto, int tipoPrecioId)
    //{
    //    if (string.IsNullOrWhiteSpace(texto))
    //        return new List<ProductoVentaDto>();

    //    var productos = await _productoRepository.BuscarAsync(texto);

    //    if (productos == null)
    //        throw new BusinessException(
    //            "Producto no encontrado.",
    //            StatusCodes.Status404NotFound);

    //    var tipoPrecio = await _catalogRepository.ObtenerTipoPrecioAsync(tipoPrecioId);

    //    if (tipoPrecio == null)
    //    {
    //        throw new BusinessException(
    //            "Tipo de precio inválido",
    //            StatusCodes.Status400BadRequest);
    //    }

    //    return productos.Select(p =>
    //    {
    //        decimal precio =
    //            tipoPrecioId == 2
    //                ? p.PrecioMayorista
    //                : p.PrecioMinorista;

    //        decimal itbis =
    //            p.AplicaItbis
    //                ? precio * (p.ItbisPorcentaje / 100m)
    //                : 0m;

    //        return new ProductoVentaDto
    //        {
    //            ProductoId = p.ProductoId,
    //            Codigo = p.Codigo,
    //            Nombre = p.Nombre,
    //            Descripcion = p.Descripcion,
    //            Precio = precio,

    //            ItbisUnitario = itbis,

    //            PrecioFinal = precio + itbis,

    //            AplicaItbis = p.AplicaItbis,

    //            ItbisPorcentaje = p.ItbisPorcentaje,

    //            Existencia = p.Existencia,

    //            Estado = p.Estado
    //        };
    //    }).ToList();
    //}
    public async Task<List<ProductoVentaDto>> BuscarProductosAsync(
    string texto,
    int tipoPrecioId)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return [];

        var productos = await _productoRepository.BuscarAsync(texto);

        if (productos == null)
            throw new BusinessException(
                "Producto no encontrado.",
                StatusCodes.Status404NotFound);

        return productos
            .Select(p => MapearProductoVentaDto(p, tipoPrecioId))
            .ToList();
    }
    private ProductoVentaDto MapearProductoVentaDto(
    Producto producto,
    int tipoPrecioId)
    {
        decimal precio =
            tipoPrecioId == 2
                ? producto.PrecioMayorista
                : producto.PrecioMinorista;

        decimal itbis =
            producto.AplicaItbis
                ? precio * (producto.ItbisPorcentaje / 100m)
                : 0m;

        return new ProductoVentaDto
        {
            ProductoId = producto.ProductoId,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,

            Precio = precio,

            ItbisUnitario = itbis,

            PrecioFinal = precio + itbis,

            AplicaItbis = producto.AplicaItbis,

            ItbisPorcentaje = producto.ItbisPorcentaje,

            Existencia = producto.Existencia,

            Estado = producto.Estado
        };
    }
    public async Task<List<ProductoVentaDto>> RecalcularProductosAsync(
    List<int> productosIds,
    int tipoPrecioId)
    {
        var productos =
            await _productoRepository.ObtenerPorIdsAsync(productosIds);

        return productos
            .Select(p => MapearProductoVentaDto(p, tipoPrecioId))
            .ToList();
    }
    public async Task<List<ClienteVentaDto>> BuscarClientesAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return new List<ClienteVentaDto>();

        var clientes = await _clienteRepository.BuscarAsync(texto);
        if (clientes == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);
        return clientes.Select(c => new ClienteVentaDto
        {
            ClienteId = c.ClienteId,
            Codigo = c.Codigo,
            Nombre = c.Nombre,
            Documento = c.Documento,
            TipoClienteId = c.TipoClienteId,
            TipoNombre = c.TipoCliente?.Nombre ?? "",
            TipoPrecioId = c.TipoPrecioId,

        }).ToList();
    }
    public async Task<ClienteVentaDto> CargarConsumidorFinalAsync()
    {
        var consumidorFinal = await _clienteRepository.CargarConsumidorFinalAsync();

        if (consumidorFinal == null)
            throw new BusinessException("Consumidor final no encontrado.", StatusCodes.Status404NotFound);

        return new ClienteVentaDto
        {
            ClienteId = consumidorFinal.ClienteId,
            Codigo = consumidorFinal.Codigo,
            Nombre = consumidorFinal.Nombre,
            Documento = consumidorFinal.Documento,
            TipoClienteId = consumidorFinal.TipoClienteId,
            TipoNombre = consumidorFinal.TipoCliente?.Nombre ?? "",
            TipoPrecioId = consumidorFinal.TipoPrecioId
        };
    }
}



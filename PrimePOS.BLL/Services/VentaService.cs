using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IFacturaService _facturaService;
    private readonly IUnitOfWork _unitOfWork;
    public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository, IFacturaService facturaService, IUnitOfWork unitOfWork)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _facturaService = facturaService;
        _unitOfWork = unitOfWork;
    }


    public async Task<VentaResponseDto> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto)
    {
        if (dto.TurnoId == 0)
            throw new BusinessException("Turno inválido.", 400);

        if (!dto.Items.Any())
            throw new BusinessException("La venta no tiene productos.", 404);


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var ahora = DateTime.Now;

            var venta = new Venta
            {
                TurnoId = dto.TurnoId,
                UsuarioId = userId,
                UsuarioNombre = nombre,
                ClienteId = dto.ClienteId,
                ClienteNombre = dto.ClienteNombre,
                MetodoPagoId = dto.MetodoPagoId,
                FechaRegistro = ahora,
                Detalles = new List<VentaDetalle>(),

                Subtotal = dto.Subtotal,
                Impuesto = dto.Impuesto,
                Descuento = dto.Descuento,
                Efectivo = dto.Efectivo,
                Cambio = dto.Cambio,
                Total = dto.Total,

                Estado = true,
            };

            decimal subtotal = 0;

            foreach (var item in dto.Items)
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(item.ProductoId);

                if (producto == null)
                    throw new BusinessException($"Producto no existe {producto!.Nombre}", 404);
                if (producto!.Existencia < item.Cantidad)
                    throw new BusinessException($"Stock insuficiente {producto.Nombre}", 400);

                producto.Existencia -= item.Cantidad;
                _productoRepository.Actualizar(producto);



                var totalItem = item.Cantidad * item.PrecioUnitario;
                subtotal += totalItem;
                venta.Detalles.Add(new VentaDetalle
                {
                    Codigo = item.Codigo,
                    ProductoId = item.ProductoId,
                    ProductoNombre = item.ProductoNombre,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Total = totalItem
                });
            }




            venta.Subtotal = subtotal;
            venta.Impuesto = subtotal * 0.18m;
            venta.Total = venta.Subtotal + venta.Impuesto;
            venta.Efectivo = venta.Efectivo;
            venta.Cambio = venta.Efectivo - venta.Total;

            _ventaRepository.Crear(venta);
            await _ventaRepository.GuardarCambiosAsync();

            var (factura, pdfUrl) = await _facturaService.GenerarFacturaDesdeVenta(venta.VentaId);

            await _unitOfWork.CommitAsync();

            return new VentaResponseDto
            {
                VentaId = venta.VentaId,
                NumeroFactura = factura.Numero,
                FileName = $"factura-{factura.Numero}.pdf",
                Fecha = venta.FechaRegistro,
                Total = venta.Total
            };
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<List<VentaDto>> ObtenerVentasPorTurnoAsync(int turnoId)
    {
        var ventas = await _ventaRepository.ObtenerPorTurnoAsync(turnoId);

        return ventas.Select(v => new VentaDto
        {
            VentaId = v.VentaId,
            FechaRegistro = v.FechaRegistro,
            ClienteNombre = v.ClienteNombre,
            MetodoPagoId = v.MetodoPagoId,
            Total = v.Total
        }).ToList();
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

}

using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.DAL.Repositories;
using PrimePOS.DAL.UnitOfWork;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class VentaService
{
    private readonly VentaRepository _ventaRepository;
    private readonly ProductoRepository _productoRepository;
    private readonly UnitOfWork _unitOfWork;
    public VentaService(VentaRepository ventaRepository, ProductoRepository productoRepository, UnitOfWork unitOfWork)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<int> CrearVentaAsync(CrearVentaDto dto)
    {
        if (dto.TurnoId == 0)
            throw new Exception("Turno inválido.");

        if (!dto.Items.Any())
            throw new Exception("La venta no tiene productos.");


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var ahora = DateTime.Now;

            var venta = new Venta
            {
                TurnoId = dto.TurnoId,
                UsuarioId = dto.UsuarioId,
                ClienteId = dto.ClienteId,
                MetodoPagoId = dto.MetodoPagoId,
                FechaRegistro = ahora,
                Detalles = new List<VentaDetalle>(),

                Subtotal = dto.Subtotal,
                Impuesto = dto.Impuesto,
                Descuento = dto.Descuento,
                Total = dto.Total,

                Estado = true,
            };

            decimal subtotal = 0;

            foreach (var item in dto.Items)
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(item.ProductoId);

                if (producto == null)
                    throw new Exception($"Producto no existe {producto!.Nombre}");
                if (producto!.Existencia < item.Cantidad)
                    throw new Exception($"Stock insuficiente {producto.Nombre}");

                producto.Existencia -= item.Cantidad;
                _productoRepository.Actualizar(producto);



                var totalItem = item.Cantidad * item.PrecioUnitario;
                subtotal += totalItem;
                venta.Detalles.Add(new VentaDetalle
                {
                    Codigo = item.Codigo,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Total = totalItem
                });
            }




            venta.Subtotal = subtotal;
            venta.Impuesto = subtotal * 0.18m;
            venta.Total = venta.Subtotal + venta.Impuesto;

            _ventaRepository.Crear(venta);
            await _ventaRepository.GuardarCambiosAsync();

            await _unitOfWork.CommitAsync();

            return venta.VentaId;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

}

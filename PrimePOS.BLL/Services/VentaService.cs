using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.DAL.Repositories;
using System.Collections.ObjectModel;

public class VentaService
{
    private readonly VentaRepository _ventaRepository;
    private readonly ProductoRepository _productoRepository;

    public VentaService(VentaRepository ventaRepository, ProductoRepository productoRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
    }

    public ObservableCollection<CarritoItem> Carrito { get; set; } = new();

    public decimal Subtotal => Carrito.Sum(x => x.Total);

    public decimal Impuesto => Subtotal * 0.18m;
    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Total => Subtotal + Impuesto - DescuentoMonto;

    public async Task AgregarProductoCarrito(int productoId)
    {
        var existente = Carrito.FirstOrDefault(p => p.ProductoId == productoId);

        if (existente != null)
        {
            existente.Cantidad++;

        }
        else
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(productoId);
            if (producto == null)
                return;

            Carrito.Add(new CarritoItem
            {
                ProductoId = producto.ProductoId,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.PrecioVenta,
                Cantidad = 1
            });
        }
    }
    public void EliminarProducto(int productoId)
    {
        var item = Carrito.FirstOrDefault(x => x.ProductoId == productoId);

        if (item != null)
        {
            Carrito.Remove(item);
        }
    }
    public void VaciarCarrito()
    {
        Carrito.Clear();
    }
    public void AplicarDescuento(decimal porcentaje)
    {
        DescuentoPorcentaje = porcentaje;
    }
    public void CalcularTotales()
    {
        decimal subtotal = Carrito.Sum(x => x.Total);

        decimal descuento = subtotal * (DescuentoPorcentaje / 100);

        decimal total = subtotal - descuento;

    }

}

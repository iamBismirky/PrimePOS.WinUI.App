using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.DAL.Repositories;
using System.Collections.ObjectModel;

public class VentaService
{
    private readonly VentaRepository _ventaRepository;

    public VentaService(VentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public ObservableCollection<CarritoItem> Carrito { get; set; } = new();

    public decimal Subtotal => Carrito.Sum(x => x.Total);

    public decimal Impuesto => Subtotal * 0.18m;
    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Total => Subtotal + Impuesto - DescuentoMonto;

    public void AgregarProductoCarrito(ProductoDto producto)
    {
        var existente = Carrito.FirstOrDefault(p => p.ProductoId == producto.ProductoId);

        if (existente != null)
        {
            existente.Cantidad++;

        }
        else
        {
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

using PrimePOS.BLL.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public class VentaViewModel : INotifyPropertyChanged
{
    private readonly VentaService _ventaService;
    private readonly ProductoService _productoService;

    public VentaViewModel(VentaService ventaService, ProductoService productoService)
    {
        _ventaService = ventaService;
        _productoService = productoService;

    }

    public ObservableCollection<CarritoItemViewModel> Carrito { get; set; } = new();

    private decimal _descuentoPorcentaje;

    public decimal Subtotal => Carrito.Sum(x => x.Total);

    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Impuesto => SubtotalConDescuento * 0.18m;

    public decimal Total => Subtotal + Impuesto - DescuentoMonto;

    public async Task AgregarProducto(int productoId)
    {
        var producto = await _productoService.ObtenerProductoPorIdAsync(productoId);
        if (producto == null) return;

        var existente = Carrito.FirstOrDefault(p => p.ProductoId == producto.ProductoId);

        if (existente != null)
        {
            existente.Cantidad++;
            NotificarTotales();
            return;
        }


        Carrito.Add(new CarritoItemViewModel
        {
            Codigo = producto.Codigo,
            ProductoId = producto.ProductoId,
            Nombre = producto.Nombre,
            Precio = producto.PrecioVenta,
            Cantidad = 1
        });


        NotificarTotales();
    }
    public void EliminarProducto(int productoId)
    {
        var item = Carrito.FirstOrDefault(x => x.ProductoId == productoId);

        if (item != null)
            Carrito.Remove(item);

        NotificarTotales();
    }
    // 🧹 LIMPIAR
    public void VaciarCarrito()
    {
        Carrito.Clear();
        NotificarTotales();
    }
    private void NotificarTotales()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(DescuentoMonto));
        OnPropertyChanged(nameof(SubtotalConDescuento));
        OnPropertyChanged(nameof(Impuesto));
        OnPropertyChanged(nameof(DescuentoPorcentaje));
        OnPropertyChanged(nameof(Total));
    }
    public void AplicarDescuento(decimal porcentaje)
    {
        DescuentoPorcentaje = porcentaje;
        NotificarTotales();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
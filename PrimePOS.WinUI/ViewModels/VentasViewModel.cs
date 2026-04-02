using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.BLL.DTOs.VentaDetalle;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Infrastructure;
using System;
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
    private readonly SesionService _sesion;
    public SesionService AppSesion => _sesion;

    public VentaViewModel(VentaService ventaService,
            ProductoService productoService,
                SesionService sesionService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _sesion = sesionService;
    }

    public ObservableCollection<CarritoItemViewModel> Carrito { get; set; } = new();



    public decimal Subtotal => Carrito.Sum(x => x.Total);

    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Impuesto => SubtotalConDescuento * 0.18m;

    public decimal Total => Subtotal + Impuesto - DescuentoMonto;

    public async Task AgregarProductoCarrito(ProductoDto dto)
    {


        var existente = Carrito.FirstOrDefault(p => p.ProductoId == dto.ProductoId);
        if (existente != null)
        {
            existente.Cantidad++;
            NotificarTotales();

        }
        else
        {
            Carrito.Add(new CarritoItemViewModel
            {
                Codigo = dto.Codigo,
                ProductoId = dto.ProductoId,
                Nombre = dto.Nombre,
                Precio = dto.PrecioVenta,
                Cantidad = 1
            });
        }
        NotificarTotales();
    }
    public void EliminarProductoCarrito(int productoId)
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
    public async Task FacturarAsync(int clienteId, int MetodoPagoId)
    {
        try
        {
            if (!Carrito.Any())
                throw new Exception("El carrito está vacío.");


            if (Sesion.TurnoActual == null)
                throw new Exception("No hay turno abierto.");

            var dto = new CrearVentaDto
            {
                ClienteId = clienteId,
                UsuarioId = Sesion.UsuarioId,
                MetodoPagoId = MetodoPagoId,
                TurnoId = Sesion.TurnoActual!.TurnoId,

                Subtotal = Subtotal,
                Impuesto = Impuesto,
                Descuento = DescuentoMonto,
                Total = Total,



                Items = Carrito.Select(x => new VentaDetalleDto
                {
                    Codigo = x.Codigo,
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.Precio,
                    Total = x.Total,

                }).ToList(),
            };
            // 🔥 LLAMADA AL SERVICE
            var ventaId = await _ventaService.CrearVentaAsync(dto);

            // 🧹 LIMPIAR CARRITO
            VaciarCarrito();

            // 🔄 RESETEAR DESCUENTO
            DescuentoPorcentaje = 0;



            System.Diagnostics.Debug.WriteLine($"Venta creada: {ventaId}");

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al facturar: {ex.Message}");
            throw;
        }



    }
}
using PrimePOS.BLL.DTOs.Venta;
using PrimePOS.BLL.DTOs.VentaDetalle;
using PrimePOS.BLL.Services;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Reportes.PrimePOS.WinUI.Reportes;
using QuestPDF.Fluent;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public class VentaViewModel : INotifyPropertyChanged
{
    private readonly VentaService _ventaService;
    private readonly ProductoService _productoService;
    private readonly AppSesionViewModel _sesion;
    private readonly FacturaService _facturaService;

    public AppSesionViewModel AppSesion => _sesion;

    public VentaViewModel(VentaService ventaService,
            ProductoService productoService,
                AppSesionViewModel sesionService,
                FacturaService facturaService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _sesion = sesionService;
        _facturaService = facturaService;
    }

    public ObservableCollection<CarritoItemViewModel> Carrito { get; set; } = new();



    public decimal Subtotal => Carrito.Sum(x => x.Total);

    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoMonto => Subtotal * (DescuentoPorcentaje / 100);
    public decimal SubtotalConDescuento => Subtotal - DescuentoMonto;
    public decimal Impuesto => SubtotalConDescuento * 0.18m;

    public decimal Total => Subtotal + Impuesto - DescuentoMonto;

    public async Task AgregarProductoCarritoAsync(ProductoDto dto)
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
    public async Task FacturarAsync(int clienteId, string clienteNombre, int metodoPagoId, decimal efectivo, decimal cambio)
    {
        try
        {
            if (!Carrito.Any())
                throw new Exception("El carrito está vacío.");


            if (AppSesion.TurnoActual == null)
                throw new Exception("No hay turno abierto.");

            var ventaDto = new CrearVentaDto
            {
                ClienteId = clienteId,
                ClienteNombre = clienteNombre,
                UsuarioId = AppSesion.TurnoActual.UsuarioId,
                UsuarioNombre = AppSesion.TurnoActual.UsuarioNombre,
                MetodoPagoId = metodoPagoId,
                TurnoId = AppSesion.TurnoActual.TurnoId,

                Subtotal = Subtotal,
                Impuesto = Impuesto,
                Descuento = DescuentoMonto,
                Efectivo = efectivo,
                Cambio = cambio,
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

            var ventaId = await _ventaService.CrearVentaAsync(ventaDto);
            var factura = await _facturaService.GenerarFacturaDesdeVenta(ventaId);

            var facturaDto = _facturaService.MapearFactura(factura);

            // Generar PDF
            var document = new Factura80mmDocument(facturaDto);

            string carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PrimePOS", "Facturas");

            Directory.CreateDirectory(carpeta);

            string path = Path.Combine(carpeta, $"Factura_{facturaDto.Numero}.pdf");

            document.GeneratePdf(path);

            // 6️⃣ Abrir PDF
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });

            VaciarCarrito();


            DescuentoPorcentaje = 0;

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al facturar: {ex.Message}");
            throw;
        }



    }
    public Action<object>? MostrarOverlay;
    public Action? CerrarOverlay;
    public async Task AbrirCobrarAsync()
    {

        if (!Carrito.Any())
            return;


        if (AppSesion.TurnoActual == null)
            return;

        var vm = new CobrarViewModel(Total);

        MostrarOverlay?.Invoke(vm);


    }

}
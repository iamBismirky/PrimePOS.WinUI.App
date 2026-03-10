using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.ENTITIES.Models;

public class DetalleVenta
{
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public string NombreProducto { get; set; } = string.Empty;

   
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }

    public DateTime FechaRegistro { get; set; }
}

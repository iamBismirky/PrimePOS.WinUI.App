using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("DetalleVentas")]
public class DetalleVenta
{
    [Key]
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }


    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Ventas")]
public class Venta
{
    [Key]
    public int VentaId { get; set; }
    public DateTime FechaRegistro { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public int MetodoPagoId { get; set; }
    public MetodoPago? MetodoPago { get; set; }

    public int TurnoId { get; set; }
    public Turno? Turno { get; set; }

    public string NumeroComprobante { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public bool Estado { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

}

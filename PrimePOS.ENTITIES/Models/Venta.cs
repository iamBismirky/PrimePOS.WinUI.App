using PrimePOS.Contracts.Enums;
using PrimePOS.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Ventas")]
public class Venta
{
    [Key]
    public int VentaId { get; set; }
    public DateTime FechaRegistro { get; set; }

    public TipoVenta TipoVenta { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public string UsuarioNombre { get; set; } = "";
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public string ClienteNombre { get; set; } = "";

    public int MetodoPagoId { get; set; }
    public MetodoPago? MetodoPago { get; set; }

    public int TurnoId { get; set; }
    public Turno? Turno { get; set; }

    public int CajaId { get; set; }
    public string NumeroComprobante { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal Cambio { get; set; }
    public decimal BalancePendiente { get; set; }

    public EstadoPago EstadoPago { get; set; }
    public EstadoVenta EstadoVenta { get; set; }
    public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();

}

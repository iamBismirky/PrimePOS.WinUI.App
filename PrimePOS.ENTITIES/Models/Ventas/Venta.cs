using PrimePOS.ENTITIES.Models.Caja;
using PrimePOS.ENTITIES.Models.Clientes;
using PrimePOS.ENTITIES.Models.Seguridad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Ventas;

[Table("Ventas")]
public class Venta
{
    [Key]
    public int VentaId { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int TipoVentaId { get; set; }
    public string TipoVentaNombre { get; set; } = "";
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = "";
    public int? ClienteId { get; set; }
    public string ClienteNombre { get; set; } = "";
    public int MetodoPagoId { get; set; }
    public string MetodoPagoNombre { get; set; } = "";
    public int TurnoId { get; set; }
    public int NumeroTurno { get; set; }
    public int? TipoPrecioId { get; set; }
    public string TipoPrecioNombre { get; set; } = "";
    public int CajaId { get; set; }
    public string CajaNombre { get; set; } = "";
    public string NumeroComprobante { get; set; } = "";
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public decimal MontoRecibido { get; set; }
    public decimal Cambio { get; set; }
    public decimal BalancePendiente { get; set; }
    public int EstadoVentaId { get; set; }
    public string EstadoVentaNombre { get; set; } = "";

    #region Navigation
    public MetodoPago? MetodoPago { get; set; }
    public TipoPrecio? TipoPrecio { get; set; }
    public Turno? Turno { get; set; }
    public EstadoVenta? EstadoVenta { get; set; }
    public Cliente? Cliente { get; set; }
    public Usuario? Usuario { get; set; }
    public TipoVenta? TipoVenta { get; set; }

    #endregion

    #region Relaciones
    public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();

    #endregion



}

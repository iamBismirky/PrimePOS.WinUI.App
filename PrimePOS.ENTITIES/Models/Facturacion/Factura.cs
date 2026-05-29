using PrimePOS.ENTITIES.Models.Clientes;
using PrimePOS.ENTITIES.Models.Seguridad;
using PrimePOS.ENTITIES.Models.Ventas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Facturacion
{
    [Table("Facturas")]
    public class Factura
    {
        [Key]
        public int FacturaId { get; set; }
        public string Numero { get; set; } = "";
        public DateTime Fecha { get; set; }
        public int VentaId { get; set; }
        public string TipoFactura { get; set; } = "";
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public int TurnoId { get; set; }
        public int NumeroTurno { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = "";
        public int MetodoPagoId { get; set; }
        public string? MetodoPago { get; set; }
        public decimal BalancePendiente { get; set; }

        #region Navigation
        public Cliente? Cliente { get; set; }
        public Usuario? Usuario { get; set; }
        public Venta? Venta { get; set; }


        #endregion
        public ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();

    }
}

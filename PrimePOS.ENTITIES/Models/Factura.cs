using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("Facturas")]
    public class Factura
    {
        [Key]
        public int FacturaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

        public int VentaId { get; set; }
        public Venta? Venta { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }

        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = "Activa";
        public string MetodoPago { get; set; } = string.Empty;
        public ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();

    }
}

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

        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }

        public string MetodoPago { get; set; } = string.Empty;
        public ICollection<FacturaDetalle> FacturasDetalle { get; set; } = new List<FacturaDetalle>();

    }
}

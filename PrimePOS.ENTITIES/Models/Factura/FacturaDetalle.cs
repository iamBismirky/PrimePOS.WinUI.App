using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Ventas
{
    [Table("FacturaDetalles")]
    public class FacturaDetalle
    {
        [Key]
        public int FacturaDetalleId { get; set; }

        public int FacturaId { get; set; }
        public Factura? Factura { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public string ProductoNombre { get; set; } = "";
        public string Codigo { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Itbis { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }
    }
}

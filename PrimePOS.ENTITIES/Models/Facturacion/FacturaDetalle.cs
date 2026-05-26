using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Facturacion
{
    [Table("FacturaDetalles")]
    public class FacturaDetalle
    {
        [Key]
        public int FacturaDetalleId { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal Itbis { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }


        #region Navigation
        public Producto? Producto { get; set; }
        public Factura? Factura { get; set; }

        #endregion
    }
}

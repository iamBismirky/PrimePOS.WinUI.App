using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Ventas;

[Table("VentaDetalles")]
public class VentaDetalle
{
    [Key]
    public int VentaDetalleId { get; set; }
    public int VentaId { get; set; }
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
    public Venta? Venta { get; set; }
    public Producto? Producto { get; set; }


    #endregion
}

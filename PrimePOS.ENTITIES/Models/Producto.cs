using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Productos")]
public class Producto
{
    [Key]
    public int ProductoId { get; set; }
    public string Codigo { get; set; } = "";
    public string CodigoBarra { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PorcentajeGanancia { get; set; }
    public decimal PrecioVenta { get; set; }
    public bool AplicaItbis { get; set; }
    public decimal ItbisPorcentaje { get; set; }
    public decimal Itbis { get; set; }
    public int Existencia { get; set; }
    public int ExistenciaMinimo { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();


}

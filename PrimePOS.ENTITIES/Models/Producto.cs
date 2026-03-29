using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Productos")]
public class Producto
{
    [Key]
    public int ProductoId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string CodigoBarra { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public string? NombreCategoria => Categoria != null ? Categoria.Nombre : string.Empty;
    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Existencia { get; set; }
    public int ExistenciaMinimo { get; set; }
    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }
    public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();


}

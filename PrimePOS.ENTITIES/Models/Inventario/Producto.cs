using PrimePOS.ENTITIES.Models.Inventario;
using PrimePOS.ENTITIES.Models.Ventas;
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
    public decimal PrecioCompra { get; set; }
    public decimal PorcentajeGananciaMinorista { get; set; }
    public decimal PorcentajeGananciaMayorista { get; set; }
    public decimal PrecioMinorista { get; set; }
    public decimal PrecioMayorista { get; set; }
    public bool AplicaItbis { get; set; }
    public decimal ItbisPorcentaje { get; set; }
    public int Existencia { get; set; }
    public int ExistenciaMinimo { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }



    #region Navigation
    public Categoria? Categoria { get; set; }

    #endregion

    #region Relaciones
    public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();

    #endregion
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Inventario;

[Table("Categorias")]
public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }
    public string? Nombre { get; set; }
    public string? Glyph { get; set; }
    public bool Estado { get; set; }




    #region Relaciones

    public ICollection<Producto>? Productos { get; set; }

    #endregion
}

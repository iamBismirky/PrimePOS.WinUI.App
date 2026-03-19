using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Categorias")]
public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; }

    public ICollection<Producto>? Productos { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();
    }
}

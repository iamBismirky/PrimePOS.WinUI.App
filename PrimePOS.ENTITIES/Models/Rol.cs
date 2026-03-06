using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.ENTITIES.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();
    }
}

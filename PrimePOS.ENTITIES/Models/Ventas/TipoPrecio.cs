using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Ventas
{
    [Table("TipoPrecios")]
    public class TipoPrecio
    {
        [Key]
        public int TipoPrecioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
        public bool Activo { get; set; } = true;

    }
}

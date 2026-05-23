using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("EstadoVentas")]
    public class EstadoVenta
    {
        [Key]
        public int EstadoVentaId { get; set; }
        public string Estado { get; set; } = "";
    }
}



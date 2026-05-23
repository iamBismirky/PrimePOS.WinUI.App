using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("TipoVentas")]
    public class TipoVenta
    {
        [Key]
        public int TipoVentaId { get; set; }
        public string Tipo { get; set; } = "";
    }


}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Ventas
{
    [Table("TipoVentas")]
    public class TipoVenta
    {
        [Key]
        public int TipoVentaId { get; set; }
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
    }


}

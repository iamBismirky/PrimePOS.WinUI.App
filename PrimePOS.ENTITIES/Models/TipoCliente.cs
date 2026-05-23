using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("TipoClientes")]
    public class TipoCliente
    {
        [Key]
        public int TipoClienteId { get; set; }
        public string Tipo { get; set; } = "";
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Clientes
{
    [Table("TipoClientes")]
    public class TipoCliente
    {
        [Key]
        public int TipoClienteId { get; set; }
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
    }
}

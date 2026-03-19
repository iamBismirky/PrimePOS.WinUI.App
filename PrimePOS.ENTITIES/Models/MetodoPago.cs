using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("MetodoPagos")]
public class MetodoPago
{
    [Key]
    public int MetodoPagoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Estado { get; set; }
}

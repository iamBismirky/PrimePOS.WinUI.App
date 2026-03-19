using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Cajas")]
public class Caja
{
    [Key]
    public int CajaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Estado { get; set; }
}

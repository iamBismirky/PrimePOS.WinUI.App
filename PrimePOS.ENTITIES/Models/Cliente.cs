using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Clientes")]
public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public string Documento { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}


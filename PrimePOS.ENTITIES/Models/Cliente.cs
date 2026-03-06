using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Clientes")]
public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Documento { get; set; }  // Cédula o RNC

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public bool? Estado { get; set; } = true; // Activo / Inactivo

    // Relación con Ventas
    public ICollection<Venta>? Ventas { get; set; } = new List<Venta>();
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }

    public int RolId { get; set; } //Clave foránea para la relación con la clase Rol
    public Rol? Rol { get; set; } //Propiedad de navegación para acceder al rol asociado al usuario

    public ICollection<Venta>? Ventas { get; set; } = new List<Venta>();
    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();

}

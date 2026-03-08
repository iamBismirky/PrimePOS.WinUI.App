using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.ENTITIES.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Today;

        public int RolId { get; set; } //Clave foránea para la relación con la clase Rol
        public Rol? Rol { get; set; } //Propiedad de navegación para acceder al rol asociado al usuario
        public string? NombreRol => Rol != null ? Rol.Nombre : string.Empty; //Propiedad calculada para obtener el nombre del rol
        
        public ICollection<Venta>? Ventas { get; set; } //Propiedad de navegación para acceder a las ventas realizadas por el usuario
    }
}

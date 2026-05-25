using PrimePOS.ENTITIES.Models.Caja;
using PrimePOS.ENTITIES.Models.Ventas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Seguridad;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool Estado { get; set; }
    public bool EsSuperAdmin { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int RolId { get; set; }


    #region Navigation
    public Rol? Rol { get; set; }

    #endregion

    #region Relaciones
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    #endregion



}

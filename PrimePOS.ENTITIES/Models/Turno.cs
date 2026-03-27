using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Turnos")]
public class Turno
{
    [Key]
    public int TurnoId { get; set; }
    public int CajaId { get; set; }
    public Caja? Caja { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int NumeroTurno { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime FechaOperacion { get; set; }
    public decimal MontoInicial { get; set; }
    public decimal? MontoCierre { get; set; }
    public DateTime? FechaCierre { get; set; }
    public bool EstaAbierto { get; set; }

    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public ICollection<CierreTurno> CierresTurno { get; set; } = new List<CierreTurno>();
}

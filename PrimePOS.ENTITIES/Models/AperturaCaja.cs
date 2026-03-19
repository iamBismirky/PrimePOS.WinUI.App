using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("AperturaCajas")]
public class AperturaCaja
{
    [Key]
    public int AperturaCajaId { get; set; }
    public int CajaId { get; set; }
    public Caja? Caja { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public DateTime FechaApertura { get; set; }
    public decimal MontoInicial { get; set; }
    public decimal? MontoCierre { get; set; }
    public DateTime? FechaCierre { get; set; }
    public int Turno { get; set; }
    public bool Estado { get; set; }

    public ICollection<Venta>? Ventas { get; set; }
}

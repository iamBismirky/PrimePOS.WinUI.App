using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models
{
    [Table("CierresTurno")]
    public class CierreTurno
    {
        [Key]
        public int CierreTurnoId { get; set; }
        public int TurnoId { get; set; }
        public Turno? Turno { get; set; }
        public decimal MontoInicial { get; set; }

        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }

        public decimal TotalGeneral { get; set; }

        public decimal EfectivoContado { get; set; }

        public decimal Diferencia { get; set; }
        public DateTime FechaCierre { get; set; }
    }
}

namespace PrimePOS.BLL.DTOs.Turno
{
    public class CierreTurnoDto
    {
        public int TurnoId { get; set; }
        public int NumeroTurno { get; set; }
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

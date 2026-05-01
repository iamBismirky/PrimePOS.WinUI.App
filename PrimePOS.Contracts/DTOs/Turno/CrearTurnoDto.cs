namespace PrimePOS.Contracts.DTOs.Turno
{
    public class CrearTurnoDto
    {
        public int TurnoId { get; set; }
        public int CajaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal MontoInicial { get; set; }
        public int NumeroTurno { get; set; }
        public DateTime FechaApertura { get; set; }
        public bool EstaAbierto { get; set; }
    }
}

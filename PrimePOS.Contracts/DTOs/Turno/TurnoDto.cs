namespace PrimePOS.Contracts.DTOs.Turno
{
    public class TurnoDto
    {
        public int TurnoId { get; set; }
        public int CajaId { get; set; }
        public string CajaNombre { get; set; } = "";
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public string RolNombre { get; set; } = "";
        public DateTime FechaApertura { get; set; }
        public DateTime FechaOperacion { get; set; }
        public int NumeroTurno { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal? MontoCierre { get; set; }
        public DateTime? FechaCierre { get; set; }
        public bool EstaAbierto { get; set; }
    }
}

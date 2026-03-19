namespace PrimePOS.BLL.DTOs.Caja
{
    public class TurnoDto
    {
        public int TurnoId { get; set; }
        public int CajaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaApertura { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal? MontoCierre { get; set; }
        public DateTime? FechaCierre { get; set; }
        public bool EstaAbierto { get; set; }
    }
}

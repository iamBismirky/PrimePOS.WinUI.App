namespace PrimePOS.BLL.DTOs.Caja
{
    public class AperturaCajaDto
    {
        public int AperturaCajaId { get; set; }
        public int CajaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaApertura { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal? MontoCierre { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int Turno { get; set; }
        public bool Estado { get; set; }
    }
}

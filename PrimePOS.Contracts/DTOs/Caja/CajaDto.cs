namespace PrimePOS.Contracts.DTOs.Caja
{
    public class CajaDto
    {
        public int CajaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}

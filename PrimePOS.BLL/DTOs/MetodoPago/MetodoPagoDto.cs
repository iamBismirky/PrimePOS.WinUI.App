namespace PrimePOS.BLL.DTOs.MetodoPago
{
    public class MetodoPagoDto
    {
        public int MetodoPagoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}

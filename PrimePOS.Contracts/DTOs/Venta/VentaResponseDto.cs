namespace PrimePOS.Contracts.DTOs.Venta
{
    public class VentaResponseDto
    {
        public int VentaId { get; set; }
        public string? NumeroFactura { get; set; }
        public string? FileName { get; set; }
        public string? UrlPdf { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}

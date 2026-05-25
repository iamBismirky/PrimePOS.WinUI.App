namespace PrimePOS.Contracts.DTOs.Factura
{
    public class FacturaGeneradaDto
    {
        public FacturaDto? Factura { get; set; }
        public string? PdfUrl { get; set; }
    }
}

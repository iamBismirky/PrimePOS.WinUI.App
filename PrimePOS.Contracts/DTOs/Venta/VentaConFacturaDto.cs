namespace PrimePOS.Contracts.DTOs.Venta
{
    public class VentaConFacturaDto
    {
        public int VentaId { get; set; }
        public int FacturaId { get; set; }
        public string FileName { get; set; } = "";
        public string PdfUrl { get; set; } = "";
        public string NumeroFactura { get; set; }
    }
}

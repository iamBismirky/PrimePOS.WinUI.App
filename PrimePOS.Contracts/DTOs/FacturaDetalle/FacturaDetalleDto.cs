namespace PrimePOS.Contracts.DTOs.FacturaDetalle
{
    public class FacturaDetalleDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }

    }
}

namespace PrimePOS.Contracts.DTOs.Factura
{
    public class FacturaDetalleDto
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal Itbis { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

    }
}

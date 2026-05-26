namespace PrimePOS.Contracts.DTOs.Venta
{
    public class VentaDetalleDto
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal Itbis { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}

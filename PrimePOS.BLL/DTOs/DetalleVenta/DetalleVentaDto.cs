namespace PrimePOS.BLL.DTOs.DetalleVenta
{
    public class DetalleVentaDto
    {
        public int DetalleVentaId { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}

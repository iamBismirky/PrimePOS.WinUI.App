namespace PrimePOS.Contracts.DTOs.Factura
{
    public class FacturaDto
    {
        public string? Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string? Turno { get; set; }
        public string? TipoFactura { get; set; }
        public string? ClienteNombre { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? CajaNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }
        public decimal BalancePendiente { get; set; }
        public string? MetodoPago { get; set; }
        public string? EstadoFacturaNombre { get; set; }



        public List<FacturaDetalleDto> Detalles { get; set; } = new List<FacturaDetalleDto>();
    }
}

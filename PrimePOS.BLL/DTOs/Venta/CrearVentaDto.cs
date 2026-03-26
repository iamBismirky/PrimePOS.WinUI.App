using PrimePOS.BLL.DTOs.DetalleVenta;

namespace PrimePOS.BLL.DTOs.Venta
{
    public class CrearVentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int UsuarioId { get; set; }

        public int ClienteId { get; set; }
        public int MetodoPagoId { get; set; }

        public int TurnoId { get; set; }


        public string NumeroComprobante { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public List<DetalleVentaDto> Items { get; set; } = new();

    }
}

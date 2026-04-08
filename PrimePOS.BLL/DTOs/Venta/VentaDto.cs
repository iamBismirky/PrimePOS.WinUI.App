namespace PrimePOS.BLL.DTOs.Venta
{
    public class VentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = "";

        public int MetodoPagoId { get; set; }

        public int TurnoId { get; set; }

        public string NumeroComprobante { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }

        public bool Estado { get; set; }
    }
}

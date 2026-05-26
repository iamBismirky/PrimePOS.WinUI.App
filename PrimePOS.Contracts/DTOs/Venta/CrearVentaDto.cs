namespace PrimePOS.Contracts.DTOs.Venta
{
    public class CrearVentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TipoVentaId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = "";
        public int MetodoPagoId { get; set; }
        public int TurnoId { get; set; }
        public int NumeroTurno { get; set; }
        public int CajaId { get; set; }
        public int TipoPrecioId { get; set; }
        public string NumeroComprobante { get; set; } = "";
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal BalancePendiente { get; set; }
        public decimal MontoRecibido { get; set; }
        public decimal Cambio { get; set; }
        public bool Estado { get; set; }
        public List<VentaDetalleDto> Items { get; set; } = new();

    }
}

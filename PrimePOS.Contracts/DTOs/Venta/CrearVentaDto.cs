using PrimePOS.Contracts.DTOs.VentaDetalle;
using PrimePOS.ENTITIES.Enums;

namespace PrimePOS.Contracts.DTOs.Venta
{
    public class CrearVentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public TipoVenta TipoVenta { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = "";
        public int MetodoPagoId { get; set; }
        public int TurnoId { get; set; }
        public int NumeroTurno { get; set; }
        public string NumeroComprobante { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal Cambio { get; set; }
        public bool Estado { get; set; }
        public List<VentaDetalleDto> Items { get; set; } = new();

    }
}

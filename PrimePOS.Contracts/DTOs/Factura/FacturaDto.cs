using PrimePOS.Contracts.DTOs.FacturaDetalle;

namespace PrimePOS.Contracts.DTOs.Factura
{
    public class FacturaDto
    {
        public string? Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string? Turno { get; set; }

        public string? ClienteNombre { get; set; }
        public string? UsuarioNombre { get; set; }


        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }

        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }
        public string? MetodoPago { get; set; }


        public List<FacturaDetalleDto> Detalles { get; set; } = new List<FacturaDetalleDto>();
    }
}

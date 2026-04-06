using PrimePOS.BLL.DTOs.FacturaDetalle;

namespace PrimePOS.BLL.DTOs.Factura
{
    public class FacturaDto
    {
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Turno { get; set; } = string.Empty;

        public string ClienteNombre { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;


        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }

        public decimal Efectivo { get; set; }
        public decimal Cambio { get; set; }
        public string MetodoPago { get; set; } = string.Empty;


        public List<FacturaDetalleDto> Detalles { get; set; } = new List<FacturaDetalleDto>();
    }
}

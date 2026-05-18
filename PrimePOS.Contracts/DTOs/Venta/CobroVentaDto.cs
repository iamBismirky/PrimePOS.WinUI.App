using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.VentaDetalle;

namespace PrimePOS.Contracts.DTOs.Venta
{
    public class CobroVentaDto
    {
        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public ClienteDto? Cliente { get; set; }

        public List<VentaDetalleDto> Items { get; set; } = [];
    }
}

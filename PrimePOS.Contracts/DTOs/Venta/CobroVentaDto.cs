namespace PrimePOS.Contracts.DTOs.Venta
{
    public class CobroVentaDto
    {
        public ClienteVentaDto? Cliente { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public List<CarritoItemDto> Items { get; set; } = [];
    }
}

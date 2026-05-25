namespace PrimePOS.Contracts.DTOs.Venta
{
    public class CarritoItemDto
    {
        public int ProductoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal ItbisUnitario { get; set; }

    }
}

namespace PrimePOS.Contracts.DTOs.Venta
{
    public class ProductoVentaDto
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public decimal ItbisUnitario { get; set; }

        public decimal PrecioFinal { get; set; }

        public bool AplicaItbis { get; set; }

        public decimal ItbisPorcentaje { get; set; }

        public decimal Existencia { get; set; }

        public bool Estado { get; set; }
    }
}

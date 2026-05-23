namespace PrimePOS.Contracts.DTOs.Producto
{
    public class CrearProductoDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioVentaMayorista { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public int Existencia { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal ItbisPorcentaje { get; set; }
        public decimal ItbisMonto { get; set; }
    }
}

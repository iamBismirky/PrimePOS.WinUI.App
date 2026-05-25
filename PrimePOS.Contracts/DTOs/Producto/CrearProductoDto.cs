namespace PrimePOS.Contracts.DTOs.Producto
{
    public class CrearProductoDto
    {
        public string Codigo { get; set; } = "";
        public string CodigoBarra { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int CategoriaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVentaMinorista { get; set; }
        public decimal PrecioVentaMayorista { get; set; }
        public decimal PorcentajeGananciaMinorista { get; set; }
        public decimal PorcentajeGananciaMayorista { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal ItbisPorcentaje { get; set; }
        public int Existencia { get; set; }
        public bool Estado { get; set; }

    }
}

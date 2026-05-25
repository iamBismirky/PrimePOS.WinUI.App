namespace PrimePOS.Contracts.DTOs.Producto
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string CodigoBarra { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = "";
        public decimal PrecioCompra { get; set; }
        public decimal PrecioMinorista { get; set; }
        public decimal PrecioMayorista { get; set; }
        public decimal PorcentajeGananciaMinorista { get; set; }
        public decimal PorcentajeGananciaMayorista { get; set; }
        public bool AplicaItbis { get; set; }
        public decimal ItbisPorcentaje { get; set; }
        public decimal ItbisMinorista { get; set; }
        public decimal ItbisMayorista { get; set; }
        public int Existencia { get; set; }
        public int ExistenciaMinimo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

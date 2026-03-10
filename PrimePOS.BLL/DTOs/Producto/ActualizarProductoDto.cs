namespace PrimePOS.BLL.DTOs.Producto
{
    public class ActualizarProductoDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencia { get; set; }
        public bool Estado { get; set; }
    }
}

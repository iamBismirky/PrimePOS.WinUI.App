namespace PrimePOS.Contracts.DTOs.Producto
{
    public class BuscarProductoDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }
}

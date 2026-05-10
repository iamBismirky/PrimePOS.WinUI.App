namespace PrimePOS.Contracts.DTOs.Categoria
{
    public class CategoriaDto
    {
        public int CategoriaId { get; set; }
        public string? Nombre { get; set; }
        public string? Glyph { get; set; }
        public bool Estado { get; set; }
    }
}

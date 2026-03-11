namespace PrimePOS.WinUI.ViewModel
{
    public class CarritoItemDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
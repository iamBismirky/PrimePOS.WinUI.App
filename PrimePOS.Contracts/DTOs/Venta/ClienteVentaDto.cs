namespace PrimePOS.Contracts.DTOs.Venta
{
    public class ClienteVentaDto
    {
        public int ClienteId { get; set; }
        public int TipoClienteId { get; set; }
        public int TipoPrecioId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string TipoNombre { get; set; } = "";
        public string Documento { get; set; } = "";
        public bool Estado { get; set; }
    }
}

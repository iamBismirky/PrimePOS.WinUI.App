namespace PrimePOS.Contracts.DTOs.Factura
{
    public class FacturaListadoDto
    {
        public int FacturaId { get; set; }

        public string NumeroFactura { get; set; } = string.Empty;

        public string ClienteNombre { get; set; } = string.Empty;

        public string UsuarioNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string TipoVenta { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}

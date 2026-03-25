namespace PrimePOS.BLL.DTOs.Venta
{
    public class CrearVentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int UsuarioId { get; set; }

        public int ClienteId { get; set; }
        public int MetodoPagoId { get; set; }

        public int TurnoId { get; set; }


        public string NumeroComprobante { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}

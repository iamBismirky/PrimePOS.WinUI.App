namespace PrimePOS.Contracts.DTOs.Usuario
{
    public class CambiarContraseñaDto
    {
        public int UsuarioId { get; set; }
        public string ContraseñaActual { get; set; } = string.Empty;
        public string ContraseñaNueva { get; set; } = string.Empty;
        public string Confirmar { get; set; } = string.Empty;
    }
}

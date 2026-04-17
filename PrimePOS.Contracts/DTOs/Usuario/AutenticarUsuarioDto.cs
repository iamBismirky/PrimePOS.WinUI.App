namespace PrimePOS.Contracts.DTOs.Usuario
{
    public class AutenticarUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}

namespace PrimePOS.Contracts.DTOs.Usuario
{
    public class AppSesionUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public string Token { get; set; } = "";
    }
}

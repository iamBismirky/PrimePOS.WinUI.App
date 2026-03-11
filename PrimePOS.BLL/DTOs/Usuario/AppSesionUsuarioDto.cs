namespace PrimePOS.BLL.DTOs.Usuario
{
    public class AppSesionUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
    }
}

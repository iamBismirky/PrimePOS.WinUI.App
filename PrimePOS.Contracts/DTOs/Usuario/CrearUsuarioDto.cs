namespace PrimePOS.Contracts.DTOs.Usuario
{
    public class CrearUsuarioDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int RolId { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaRegistro { get; set; }
    }
}

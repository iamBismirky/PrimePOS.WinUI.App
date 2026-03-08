using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Usuario
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int RolId { get; set; } 
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

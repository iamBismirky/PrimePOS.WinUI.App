using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Usuario
{
    public class AutenticaUsuarioDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

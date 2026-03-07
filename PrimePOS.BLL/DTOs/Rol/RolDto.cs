using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Rol
{
    public class RolDto
    {
        public int RolId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}

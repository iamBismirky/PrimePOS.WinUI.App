using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Rol
{
    public class ActualizarRolDto
    {
        public int RolId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}

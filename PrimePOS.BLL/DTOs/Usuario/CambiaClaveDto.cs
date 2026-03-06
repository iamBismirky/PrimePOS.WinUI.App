using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Usuario
{
    public class CambiarClaveDto
    {
        public string ClaveActual { get; set; } = string.Empty;

        public string ClaveNueva { get; set; } = string.Empty;
    }
}

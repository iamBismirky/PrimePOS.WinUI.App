using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.Contracts.DTOs.Rol
{
    public class ListaRolesDto
    {
        public int RolId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}

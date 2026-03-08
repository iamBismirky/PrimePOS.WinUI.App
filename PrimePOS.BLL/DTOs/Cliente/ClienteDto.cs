using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Cliente
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public bool Estado { get; set; } 

    }
}

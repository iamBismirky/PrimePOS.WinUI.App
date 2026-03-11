using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.ENTITIES.Models
{
    public class MetodoPago
    {
        public int MetodoPagoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}

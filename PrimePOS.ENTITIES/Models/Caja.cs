using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.ENTITIES.Models
{
    public class Caja
    {
        public int CajaId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}

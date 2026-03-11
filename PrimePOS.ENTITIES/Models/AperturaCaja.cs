using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.ENTITIES.Models
{
    public class AperturaCaja
    {
        public int AperturaCajaId { get; set; }

        public int CajaId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime FechaApertura { get; set; }

        public decimal MontoInicial { get; set; }

        public decimal? MontoCierre { get; set; }

        public DateTime? FechaCierre { get; set; }

        public bool Estado { get; set; }
    }
}

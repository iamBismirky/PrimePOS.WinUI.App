using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.WinUI.ViewModel
{
    public class CarritoItemVm
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = "";

        public string Nombre { get; set; } = "";

        public decimal Precio { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Total => Precio * Cantidad;
    }
}

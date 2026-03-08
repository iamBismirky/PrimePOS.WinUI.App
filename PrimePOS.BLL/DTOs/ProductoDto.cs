using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencia { get; set; }
        public int ExistenciaMinimo { get; set; }
        public bool Estado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.DTOs.Categoria
{
    public class CategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    } 
}

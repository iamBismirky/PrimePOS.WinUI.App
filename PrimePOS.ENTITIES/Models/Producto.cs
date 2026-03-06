using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PrimePOS.ENTITIES.Models;

[Table("Productos")]
public class Producto
{
        [Key]
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public string? NombreCategoria => Categoria != null ? Categoria.Nombre : string.Empty;
        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Existencia { get; set; }
        public int ExistenciaMinimo { get; set; }
        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Today;
        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

    
}

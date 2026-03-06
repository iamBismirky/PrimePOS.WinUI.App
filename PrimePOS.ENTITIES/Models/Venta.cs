using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.ENTITIES.Models;

public class Venta
{
    public int VentaId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int ClienteId { get; set; }    
    public Cliente? Cliente { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TotalImpuesto { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public string? MetodoPago { get; set; }
    public bool Estado { get; set; }    
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

}

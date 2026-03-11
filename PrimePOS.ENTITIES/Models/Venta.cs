using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.ENTITIES.Models;

public class Venta
{
    public int VentaId { get; set; }
    public DateTime FechaRegistro { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int ClienteId { get; set; }    
    public Cliente? Cliente { get; set; }

    public int CajaId { get; set; }
    public Caja? Caja { get; set; }

    public int MetodoPagoId { get; set; }
    public MetodoPago? MetodoPago { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public bool Estado { get; set; }    
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

}

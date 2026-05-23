using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models;

[Table("Clientes")]
public class Cliente
{
    [Key]
    public int ClienteId { get; set; }
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Email { get; set; } = "";
    public string Direccion { get; set; } = "";
    public int TipoClienteId { get; set; }
    public TipoCliente? TipoCliente { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}


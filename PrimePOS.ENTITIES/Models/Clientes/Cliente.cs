using PrimePOS.ENTITIES.Models.Facturacion;
using PrimePOS.ENTITIES.Models.Ventas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Clientes;

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
    public int TipoPrecioId { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }




    #region navigation 
    public TipoPrecio? TipoPrecio { get; set; }
    public TipoCliente? TipoCliente { get; set; }

    #endregion

    #region relaciones
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    #endregion
}


using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.BLL.Services;

public class DetalleVentaService
{
    private const decimal ITBIS = 0.18m;
    public decimal CalcularSubtotal(DetalleVenta detalle)
    {
        return detalle.Cantidad * detalle.PrecioUnitario;
    }
    public decimal CalcularITBIS(decimal subtotal)
    {
        return subtotal * ITBIS;
    }
    public decimal CalcularTotal(decimal subtotal)
    {
        return subtotal * ITBIS;
    }
}

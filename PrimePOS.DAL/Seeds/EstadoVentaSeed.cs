using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Seeds;

internal static class EstadoVentaSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoVenta>().HasData(
            new EstadoVenta { EstadoVentaId = 1, Estado = "Pagada", Codigo = "PAGADA" },
            new EstadoVenta { EstadoVentaId = 2, Estado = "Pendiente", Codigo = "PENDIENTE" },
            new EstadoVenta { EstadoVentaId = 3, Estado = "Parcial", Codigo = "PARCIAL" },
            new EstadoVenta { EstadoVentaId = 4, Estado = "Anulada", Codigo = "ANULADA" },
            new EstadoVenta { EstadoVentaId = 5, Estado = "Devuelta", Codigo = "DEVUELTA" });
    }
}

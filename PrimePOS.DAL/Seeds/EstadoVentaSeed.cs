using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Seeds;

internal static class EstadoVentaSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoVenta>().HasData(
            new EstadoVenta { EstadoVentaId = 1, Estado = "Pagada" },
            new EstadoVenta { EstadoVentaId = 2, Estado = "Pendiente" },
            new EstadoVenta { EstadoVentaId = 3, Estado = "Anulada" },
            new EstadoVenta { EstadoVentaId = 4, Estado = "Devuelta" });
    }
}

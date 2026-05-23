using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Seeds;

internal static class TipoVentaSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoVenta>().HasData(
            new TipoVenta { TipoVentaId = 1, Tipo = "CONTADO" },
            new TipoVenta { TipoVentaId = 2, Tipo = "CREDITO" });
    }
}

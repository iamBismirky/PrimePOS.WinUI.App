using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Seeds;

internal static class TipoVentaSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoVenta>().HasData(
            new TipoVenta { TipoVentaId = 1, Nombre = "Contado", Codigo = "CONTADO" },
            new TipoVenta { TipoVentaId = 2, Nombre = "Credito", Codigo = "CREDITO" });
    }
}

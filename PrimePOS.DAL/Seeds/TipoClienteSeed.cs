using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Seeds;

internal static class TipoClienteSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoCliente>().HasData(
            new TipoCliente { TipoClienteId = 1, Tipo = "Minorista" },
            new TipoCliente { TipoClienteId = 2, Tipo = "Mayorista" });
    }
}

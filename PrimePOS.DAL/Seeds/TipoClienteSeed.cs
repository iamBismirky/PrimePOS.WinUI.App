using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Clientes;

namespace PrimePOS.DAL.Seeds;

internal static class TipoClienteSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoCliente>().HasData(
            new TipoCliente { TipoClienteId = 1, Nombre = "Minorista", Codigo = "MINORISTA" },
            new TipoCliente { TipoClienteId = 2, Nombre = "Mayorista", Codigo = "MAYORISTA" });
    }
}

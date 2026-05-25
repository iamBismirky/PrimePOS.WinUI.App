using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Caja;

namespace PrimePOS.DAL.Seeds;

internal static class CajaSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caja>().HasData(
            new Caja { CajaId = 1, Nombre = "Principal", Estado = true });
    }
}

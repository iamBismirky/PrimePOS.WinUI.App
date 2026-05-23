using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Seeds;

internal static class RolSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "Administrador", Estado = true },
            new Rol { RolId = 2, Nombre = "Supervisor", Estado = true },
            new Rol { RolId = 3, Nombre = "Cajero", Estado = true },
            new Rol { RolId = 4, Nombre = "Tecnico", Estado = true });
    }
}

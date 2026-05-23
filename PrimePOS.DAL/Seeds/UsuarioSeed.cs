using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Seeds;

internal static class UsuarioSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                UsuarioId = 1,
                Codigo = "USER-001",
                Nombre = "Bismirky",
                Apellidos = "Mejia",
                Username = "BMEJIA",
                Password = "$2a$11$5f0tPIEArD.GCftdaP.73.k6U5uZVqoHv2t0NUudYc3IMq0xjnYey",
                RolId = 1,
                EsSuperAdmin = true,
                Estado = true,

            }
            );
    }
}

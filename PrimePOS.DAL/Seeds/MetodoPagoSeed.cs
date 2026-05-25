using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Seeds;

internal static class MetodoPagoSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MetodoPago>().HasData(
            new MetodoPago { MetodoPagoId = 1, Nombre = "Efectivo", Estado = true },
            new MetodoPago { MetodoPagoId = 2, Nombre = "Tarjeta", Estado = true },
            new MetodoPago { MetodoPagoId = 3, Nombre = "Transferencia", Estado = true });
    }
}

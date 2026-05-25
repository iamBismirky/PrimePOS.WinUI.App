using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Seeds
{
    internal class TipoPrecioSeed
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoPrecio>().HasData(
                new TipoPrecio { TipoPrecioId = 1, Nombre = "Minorista", Codigo = "MINORISTA" },
                new TipoPrecio { TipoPrecioId = 2, Nombre = "Mayorista", Codigo = "MAYORISTA" });
        }
    }
}

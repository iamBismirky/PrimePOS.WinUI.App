using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models.Clientes;

namespace PrimePOS.DAL.Seeds;

internal static class ClienteSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasData(
            new Cliente
            {
                ClienteId = 1,
                Codigo = "CLIENT-0001",
                Nombre = "Consumidor Final",
                Documento = "0000000",
                Estado = true,
                TipoClienteId = 1,
                TipoPrecioId = 1,
            });
    }
}

using Microsoft.EntityFrameworkCore;

namespace PrimePOS.DAL.Seeds;

internal static class ModelBuilderSeedExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        RolSeed.Apply(modelBuilder);
        MetodoPagoSeed.Apply(modelBuilder);
        CajaSeed.Apply(modelBuilder);
        TipoClienteSeed.Apply(modelBuilder);
        TipoVentaSeed.Apply(modelBuilder);
        EstadoVentaSeed.Apply(modelBuilder);
        ClienteSeed.Apply(modelBuilder);
        UsuarioSeed.Apply(modelBuilder);
        TipoPrecioSeed.Apply(modelBuilder);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Configurations;

public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
{
    public void Configure(EntityTypeBuilder<Factura> builder)
    {
        builder.HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Usuario)
            .WithMany(u => u.Facturas)
            .HasForeignKey(f => f.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Venta)
            .WithMany()
            .HasForeignKey(f => f.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

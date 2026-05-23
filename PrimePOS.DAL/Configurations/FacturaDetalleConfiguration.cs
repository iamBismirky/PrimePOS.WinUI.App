using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Configurations;

public class FacturaDetalleConfiguration : IEntityTypeConfiguration<FacturaDetalle>
{
    public void Configure(EntityTypeBuilder<FacturaDetalle> builder)
    {
        builder.HasOne(d => d.Factura)
            .WithMany(f => f.Detalles)
            .HasForeignKey(d => d.FacturaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

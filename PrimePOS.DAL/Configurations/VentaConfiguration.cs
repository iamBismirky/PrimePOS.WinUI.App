using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.HasOne(v => v.Usuario)
            .WithMany(u => u.Ventas)
            .HasForeignKey(v => v.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.MetodoPago)
            .WithMany()
            .HasForeignKey(v => v.MetodoPagoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.Turno)
            .WithMany(t => t.Ventas)
            .HasForeignKey(v => v.TurnoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.TipoVenta)
            .WithMany()
            .HasForeignKey(v => v.TipoVentaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.EstadoVenta)
            .WithMany()
            .HasForeignKey(v => v.EstadoVentaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.TipoPrecio)
            .WithMany()
            .HasForeignKey(v => v.TipoPrecioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

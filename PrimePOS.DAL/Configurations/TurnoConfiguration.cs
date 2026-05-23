using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Configurations;

public class TurnoConfiguration : IEntityTypeConfiguration<Turno>
{
    public void Configure(EntityTypeBuilder<Turno> builder)
    {
        builder.HasOne(t => t.Usuario)
            .WithMany(u => u.Turnos)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.Caja)
            .WithMany()
            .HasForeignKey(t => t.CajaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

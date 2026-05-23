using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasOne(c => c.TipoCliente)
            .WithMany()
            .HasForeignKey(c => c.TipoClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

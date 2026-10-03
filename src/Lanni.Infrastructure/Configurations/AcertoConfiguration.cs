using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lanni.Infrastructure.Configurations;

public class AcertoConfiguration : IEntityTypeConfiguration<Acerto>
{
    public void Configure(EntityTypeBuilder<Acerto> builder)
    {
        builder.ToTable("Acertos");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();
        builder.Property(a => a.Valor).HasPrecision(18, 2);

        builder.HasOne(a => a.De)
            .WithMany()
            .HasForeignKey(a => a.DeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Para)
            .WithMany()
            .HasForeignKey(a => a.ParaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
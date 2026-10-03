using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lanni.Infrastructure.Configurations;

public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
{
    public void Configure(EntityTypeBuilder<Grupo> builder)
    {
        builder.ToTable("Grupos");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedNever();
        builder.Property(g => g.Nome).IsRequired().HasMaxLength(100);

        builder.HasMany(g => g.Participantes)
            .WithOne(p => p.Grupo)
            .HasForeignKey(p => p.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Despesas)
            .WithOne(d => d.Grupo)
            .HasForeignKey(d => d.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Acertos)
            .WithOne(a => a.Grupo)
            .HasForeignKey(a => a.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
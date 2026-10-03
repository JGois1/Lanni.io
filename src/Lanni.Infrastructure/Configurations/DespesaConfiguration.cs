using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lanni.Infrastructure.Configurations;

public class DespesaConfiguration : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder)
    {
        builder.ToTable("Despesas");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedNever();
        builder.Property(d => d.Descricao).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Valor).HasPrecision(18, 2);

        builder.HasOne(d => d.Pagador)
            .WithMany()
            .HasForeignKey(d => d.PagadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Divisoes)
            .WithOne(x => x.Despesa)
            .HasForeignKey(x => x.DespesaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
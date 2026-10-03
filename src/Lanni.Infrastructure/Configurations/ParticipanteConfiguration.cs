using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lanni.Infrastructure.Configurations;

public class ParticipanteConfiguration : IEntityTypeConfiguration<Participante>
{
    public void Configure(EntityTypeBuilder<Participante> builder)
    {
        builder.ToTable("Participantes");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
    }
}
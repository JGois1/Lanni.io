using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lanni.Infrastructure.Configurations;

public class DivisaoDespesaConfiguration : IEntityTypeConfiguration<DivisaoDespesa>
{
    public void Configure(EntityTypeBuilder<DivisaoDespesa> builder)
    {
        builder.ToTable("DivisoesDespesa");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Valor).HasPrecision(18, 2);

        builder.HasOne(x => x.Participante)
            .WithMany()
            .HasForeignKey(x => x.ParticipanteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
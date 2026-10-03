using Lanni.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lanni.Infrastructure;

public class LanniDbContext : DbContext
{
    public LanniDbContext(DbContextOptions<LanniDbContext> options) : base(options) { }

    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<DivisaoDespesa> DivisoesDespesa => Set<DivisaoDespesa>();
    public DbSet<Acerto> Acertos => Set<Acerto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LanniDbContext).Assembly);
    }
}
using HT.Agendas.Domain.Models;
using HT.Core.Commons.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HT.Agendas.Infra.Data;

public class AgendaDbContext : DbContext, IUnitOfWork
{
    public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }

    public async Task<bool> Commit()
    {
        SetDates(ChangeTracker.Entries());
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendaDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private void SetDates(IEnumerable<EntityEntry> entries)
    {
        foreach (var entry in entries
                     .Where(entry =>
                         entry.Entity.GetType().GetProperty("DataHora") != null))
        {
            var dataHora = (DateTime)entry.Property("DataHora").CurrentValue!;

            entry.Property("DataHora").CurrentValue = dataHora.ToUniversalTime();
        }
    }
}
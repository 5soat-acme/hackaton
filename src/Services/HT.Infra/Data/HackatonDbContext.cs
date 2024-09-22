using HT.Core.Commons.Repository;
using HT.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HT.Infra.Data;

public sealed class HackatonDbContext : DbContext, IUnitOfWork
{
    public HackatonDbContext(DbContextOptions<HackatonDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HackatonDbContext).Assembly);

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
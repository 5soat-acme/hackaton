using HT.Core.Commons.Messages;
using HT.Core.Commons.Repository;
using HT.Pacientes.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HT.Pacientes.Infra.Data;

public sealed class PacienteDbContext : DbContext, IUnitOfWork
{
    public PacienteDbContext(DbContextOptions<PacienteDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Paciente> Pacientes { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PacienteDbContext).Assembly);
        modelBuilder.Ignore<Event>();

        base.OnModelCreating(modelBuilder);
    }
}
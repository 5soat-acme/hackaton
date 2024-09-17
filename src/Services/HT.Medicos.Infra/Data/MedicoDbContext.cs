using HT.Core.Commons.Messages;
using HT.Core.Commons.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using HT.Medicos.Domain.Models;

namespace HT.Medicos.Infra.Data;

public class MedicoDbContext : DbContext, IUnitOfWork
{
    public MedicoDbContext(DbContextOptions<MedicoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Medico> Medicos { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicoDbContext).Assembly);
        modelBuilder.Ignore<Event>();

        base.OnModelCreating(modelBuilder);
    }
}
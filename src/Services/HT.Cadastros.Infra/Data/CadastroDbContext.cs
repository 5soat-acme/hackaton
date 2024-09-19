using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HT.Cadastros.Infra.Data;

public sealed class CadastroDbContext : DbContext, IUnitOfWork
{
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Paciente> Pacientes { get; set; }
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CadastroDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
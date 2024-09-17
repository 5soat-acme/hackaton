using HT.Core.Commons.Repository;
using HT.Pacientes.Domain.Models;
using HT.Pacientes.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Pacientes.Infra.Data.Repository;

public sealed class PacienteRepository : IPacienteRepository
{
    private readonly PacienteDbContext _dbContext;

    public PacienteRepository(PacienteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public void Criar(Paciente paciente)
    {
        _dbContext.Pacientes.AddAsync(paciente);
    }

    public async Task<IEnumerable<Paciente>> Buscar()
    {
        return await _dbContext.Pacientes.ToListAsync();
    }
}
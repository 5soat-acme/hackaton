using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Cadastros.Infra.Data.Repository;

public sealed class PacienteRepository : IPacienteRepository
{
    private readonly CadastroDbContext _dbContext;

    public PacienteRepository(CadastroDbContext dbContext)
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
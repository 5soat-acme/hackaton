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

    public async Task Criar(Paciente paciente)
    {
        await _dbContext.Pacientes.AddAsync(paciente);
    }

    public async Task<IEnumerable<Paciente>> Buscar()
    {
        return await _dbContext.Pacientes.ToListAsync();
    }

    public async Task<Paciente?> BuscarPorEmail(string email)
    {
        return await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Paciente?> BuscarPorCpf(string cpf)
    {
        return await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Cpf.Numero == cpf);
    }
}
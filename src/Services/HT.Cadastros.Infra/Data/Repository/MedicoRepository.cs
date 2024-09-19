using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Cadastros.Infra.Data.Repository;

public class MedicoRepository : IMedicoRepository
{
    private readonly CadastroDbContext _dbContext;

    public MedicoRepository(CadastroDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task Criar(Medico medico)
    {
        await _dbContext.Medicos.AddAsync(medico);
    }

    public async Task<IEnumerable<Medico>> Buscar()
    {
        return await _dbContext.Medicos.ToListAsync();
    }
}

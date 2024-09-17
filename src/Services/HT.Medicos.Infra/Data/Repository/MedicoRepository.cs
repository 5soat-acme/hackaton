using HT.Core.Commons.Repository;
using HT.Medicos.Domain.Models;
using HT.Medicos.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Medicos.Infra.Data.Repository;

public class MedicoRepository : IMedicoRepository
{
    private readonly MedicoDbContext _dbContext;

    public MedicoRepository(MedicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public void Criar(Medico medico)
    {
        _dbContext.Medicos.AddAsync(medico);
    }

    public async Task<IEnumerable<Medico>> Buscar()
    {
        return await _dbContext.Medicos.ToListAsync();
    }
}

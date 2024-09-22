using HT.Core.Commons.Repository;
using HT.Domain.Models;
using HT.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Infra.Data.Repository;

public class MedicoRepository : IMedicoRepository
{
    private readonly HackatonDbContext _dbContext;

    public MedicoRepository(HackatonDbContext dbContext)
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

    public async Task<Medico?> BuscarPorId(Guid medicoId)
    {
        return await _dbContext.Medicos.FirstOrDefaultAsync(x => x.Id == medicoId);
    }
}

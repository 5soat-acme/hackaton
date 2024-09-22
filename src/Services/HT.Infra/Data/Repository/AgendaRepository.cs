using HT.Core.Commons.Repository;
using HT.Domain.Models;
using HT.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Infra.Data.Repository;

public class AgendaRepository : IAgendaRepository
{
    private readonly HackatonDbContext _dbContext;

    public AgendaRepository(HackatonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task Criar(Agenda agenda)
    {
        await _dbContext.Agendas.AddAsync(agenda);
    }

    public void Remover(Agenda agenda)
    {
        _dbContext.Agendas.Remove(agenda);
    }

    public async Task<Agenda?> BuscarPorId(Guid agendaId)
    {
        return await _dbContext.Agendas.FirstOrDefaultAsync(x => x.Id == agendaId);
    }

    public async Task<Agenda?> BuscarPorIdEMedicoId(Guid agendaId, Guid medicoId)
    {
        return await _dbContext.Agendas.FirstOrDefaultAsync(x => x.Id == agendaId && x.MedicoId == medicoId);
    }

    public async Task<IEnumerable<Agenda>> BuscarDisponivelPorMedico(Guid medicoId)
    {
        return await _dbContext.Agendas
            .Where(x => x.MedicoId == medicoId && !_dbContext.Agendamentos.Any(k => k.AgendaId == x.Id))
            .ToListAsync();
    }

    public async Task<Agenda?> BuscarPorMedicoHora(Guid medicoId, DateTime dataHora)
    {
        return await _dbContext.Agendas.FirstOrDefaultAsync(x => x.MedicoId == medicoId && x.DataHora == dataHora.ToUniversalTime());
    }
}

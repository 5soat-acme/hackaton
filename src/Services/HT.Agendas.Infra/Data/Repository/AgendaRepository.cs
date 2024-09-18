using HT.Agendas.Domain.Models;
using HT.Agendas.Domain.Repository;
using HT.Core.Commons.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Agendas.Infra.Data.Repository;

public class AgendaRepository : IAgendaRepository
{
    private readonly AgendaDbContext _dbContext;

    public AgendaRepository(AgendaDbContext dbContext)
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
        return await _dbContext.Agendas.FirstOrDefaultAsync(produto => produto.Id == agendaId);
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

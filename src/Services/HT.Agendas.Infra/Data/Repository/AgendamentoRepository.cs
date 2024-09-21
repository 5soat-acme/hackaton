using HT.Agendas.Domain.Models;
using HT.Agendas.Domain.Repository;
using HT.Core.Commons.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Agendas.Infra.Data.Repository;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly AgendaDbContext _dbContext;

    public AgendamentoRepository(AgendaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task Criar(Agendamento agendamento)
    {
        await _dbContext.Agendamentos.AddAsync(agendamento);
    }

    public async Task<Agendamento?> BuscarPorAgenda(Guid agendaId)
    {
        return await _dbContext.Agendamentos.FirstOrDefaultAsync(x => x.AgendaId == agendaId);
    }
}
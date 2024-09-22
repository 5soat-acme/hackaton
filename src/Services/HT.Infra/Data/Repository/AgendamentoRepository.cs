using HT.Core.Commons.Repository;
using HT.Domain.Models;
using HT.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Infra.Data.Repository;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly HackatonDbContext _dbContext;

    public AgendamentoRepository(HackatonDbContext dbContext)
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
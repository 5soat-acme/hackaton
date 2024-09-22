using HT.Core.Commons.Repository;
using HT.Domain.Models;

namespace HT.Domain.Repository;

public interface IAgendamentoRepository : IRepository<Agendamento>
{
    Task Criar(Agendamento agendamento);
    Task<Agendamento?> BuscarPorAgenda(Guid agendaId);
}
using HT.Agendas.Domain.Models;
using HT.Core.Commons.Repository;

namespace HT.Agendas.Domain.Repository;

public interface IAgendamentoRepository : IRepository<Agendamento>
{
    Task Criar(Agendamento agendamento);
    Task<Agendamento?> BuscarPorAgenda(Guid agendaId);
}
using HT.Agendas.Domain.Models;
using HT.Core.Commons.Repository;

namespace HT.Agendas.Domain.Repository;

public interface IAgendaRepository : IRepository<Agenda>
{
    Task Criar(Agenda agenda);
    void Remover(Agenda agenda);
    Task<Agenda?> BuscarPorId(Guid agendaId);
    Task<IEnumerable<Agenda>> BuscarDisponivelPorMedico(Guid medicoId);
    Task<Agenda?> BuscarPorMedicoHora(Guid medicoId, DateTime dataHora);
}
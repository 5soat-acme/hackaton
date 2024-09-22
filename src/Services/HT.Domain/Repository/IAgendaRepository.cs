using HT.Core.Commons.Repository;
using HT.Domain.Models;

namespace HT.Domain.Repository;

public interface IAgendaRepository : IRepository<Agenda>
{
    Task Criar(Agenda agenda);
    void Remover(Agenda agenda);
    Task<Agenda?> BuscarPorId(Guid agendaId);
    Task<Agenda?> BuscarPorIdEMedicoId(Guid agendaId, Guid medicoId);
    Task<IEnumerable<Agenda>> BuscarDisponivelPorMedico(Guid medicoId);
    Task<Agenda?> BuscarPorMedicoHora(Guid medicoId, DateTime dataHora);
}
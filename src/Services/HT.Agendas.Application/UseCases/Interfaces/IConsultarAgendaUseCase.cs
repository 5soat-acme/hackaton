using HT.Agendas.Application.DTOs.Responses;

namespace HT.Agendas.Application.UseCases.Interfaces;

public interface IConsultarAgendaUseCase
{
    Task<IEnumerable<AgendaDto>?> BuscarDisponivelPorMedico(Guid medicoId);
}

using HT.Application.DTOs.Responses;

namespace HT.Application.UseCases.Interfaces;

public interface IConsultarAgendaUseCase
{
    Task<IEnumerable<AgendaDto>?> BuscarDisponivelPorMedico(Guid medicoId);
}

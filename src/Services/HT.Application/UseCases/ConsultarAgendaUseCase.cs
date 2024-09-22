using HT.Application.DTOs.Responses;
using HT.Application.Mappings;
using HT.Application.UseCases.Interfaces;
using HT.Domain.Repository;

namespace HT.Application.UseCases;

public class ConsultarAgendaUseCase: IConsultarAgendaUseCase
{
    private readonly IAgendaRepository _agendaRepository;

    public ConsultarAgendaUseCase(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public async Task<IEnumerable<AgendaDto>?> BuscarDisponivelPorMedico(Guid medicoId)
    {
        var agendas = await _agendaRepository.BuscarDisponivelPorMedico(medicoId);
        return AgendaDomainToDtoMapper.Map(agendas);
    }
}
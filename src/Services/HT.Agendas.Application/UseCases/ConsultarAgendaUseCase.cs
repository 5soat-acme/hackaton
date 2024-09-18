using HT.Agendas.Application.DTOs.Responses;
using HT.Agendas.Application.Mappings;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Repository;

namespace HT.Agendas.Application.UseCases;

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
        return DomainToDtoMapper.Map(agendas);
    }
}
using HT.Agendas.Application.DTOs.Responses;
using HT.Agendas.Domain.Models;

namespace HT.Agendas.Application.Mappings;

public class DomainToDtoMapper
{
    public static AgendaDto Map(Agenda agenda)
    {
        return new AgendaDto
        {
            Id = agenda.Id,
            DataHora = agenda.DataHora
        };
    }

    public static IEnumerable<AgendaDto> Map(IEnumerable<Agenda> agendas)
    {
        return agendas.Select(Map).ToList();
    }
}
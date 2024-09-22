using HT.Application.DTOs.Responses;
using HT.Domain.Models;

namespace HT.Application.Mappings;

public class AgendaDomainToDtoMapper
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
using System.Text.Json.Serialization;

namespace HT.Agendas.Application.DTOs.Requests;

public class CriarAgendaDto
{
    [JsonIgnore]
    public Guid MedicoId { get; set; }
    public DateTime DataHora { get; set; }
}
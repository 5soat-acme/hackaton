using System.Text.Json.Serialization;

namespace HT.Application.DTOs.Requests;

public class CriarAgendamentoDto
{
    [JsonIgnore]
    public Guid PacienteId { get; set; }
    public Guid AgendaId { get; set; }    
}
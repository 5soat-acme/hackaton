using HT.Core.Commons.DomainObjects;

namespace HT.Domain.Models;

public class Agendamento : Entity, IAggregateRoot
{
    private Agendamento() { }

    public Agendamento(Guid agendaId, Guid pacienteId)
    {
        ValidarAgendamento(agendaId, pacienteId);

        AgendaId = agendaId;
        PacienteId = pacienteId;
    }

    public Guid AgendaId { get; private set; }
    public Guid PacienteId { get; private set; }

    public void ValidarAgendamento(Guid agendaId, Guid pacienteId)
    {
        ValidarAgenda(agendaId);
        ValidarPaciente(pacienteId);
    }

    private void ValidarAgenda(Guid agendaId)
    {
        if (agendaId == Guid.Empty) throw new DomainException("Agenda inválida");
    }

    private void ValidarPaciente(Guid pacienteId)
    {
        if (pacienteId == Guid.Empty) throw new DomainException("Paciente inválido");
    }
}
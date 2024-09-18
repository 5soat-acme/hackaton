using HT.Core.Commons.DomainObjects;

namespace HT.Agendas.Domain.Models;

public class Agenda : Entity, IAggregateRoot
{
    private Agenda() { }

    public Agenda(Guid medicoId, DateTime dataHora)
    {
        ValidarAgenda(medicoId, dataHora);

        MedicoId = medicoId;
        DataHora = dataHora;
    }

    public Guid MedicoId { get; private set; }
    public DateTime DataHora { get; private set; }

    public void ValidarAgenda(Guid medicoId, DateTime dataHora)
    {
        ValidarMedico(medicoId);
        ValidarDataHora(dataHora);
    }

    private void ValidarMedico(Guid medicoId)
    {
        if (medicoId == Guid.Empty) throw new DomainException("Médico inválido");
    }

    private void ValidarDataHora(DateTime dataHora)
    {
        if (dataHora.Minute != 0 || dataHora.Second != 0 || dataHora.Millisecond != 0)
            throw new DomainException("Data/Hora inválida");
    }
}
using HT.Application.UseCases.Interfaces;
using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using HT.Domain.Repository;
using System.ComponentModel.DataAnnotations;

namespace HT.Application.UseCases;

public class RemoverAgendaUseCase : CommonUseCase, IRemoverAgendaUseCase
{
    private readonly IAgendaRepository _agendaRepository;

    public RemoverAgendaUseCase(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public async Task<OperationResult> Handle(Guid id, Guid medicoId)
    {
        var agenda = await _agendaRepository.BuscarPorIdEMedicoId(id, medicoId);
        if (agenda is null) throw new ValidationException("Agenda não existe");
        _agendaRepository.Remover(agenda!);
        await PersistData(_agendaRepository.UnitOfWork);
        return OperationResult.Success();
    }
}
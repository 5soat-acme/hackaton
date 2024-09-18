﻿using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Repository;
using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using System.ComponentModel.DataAnnotations;

namespace HT.Agendas.Application.UseCases;

public class RemoverAgendaUseCase : CommonUseCase, IRemoverAgendaUseCase
{
    private readonly IAgendaRepository _agendaRepository;

    public RemoverAgendaUseCase(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public async Task<OperationResult> Handle(Guid id)
    {
        var agenda = await _agendaRepository.BuscarPorId(id);
        if (agenda is null) throw new ValidationException("Agenda não existe");
        _agendaRepository.Remover(agenda!);
        await PersistData(_agendaRepository.UnitOfWork);
        return OperationResult.Success();
    }
}
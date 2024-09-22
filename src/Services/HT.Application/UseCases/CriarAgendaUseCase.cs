using HT.Application.DTOs.Requests;
using HT.Application.UseCases.Interfaces;
using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using HT.Domain.Models;
using HT.Domain.Repository;

namespace HT.Application.UseCases;

public class CriarAgendaUseCase : CommonUseCase, ICriarAgendaUseCase
{
    private readonly IAgendaRepository _agendaRepository;

    public CriarAgendaUseCase(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CriarAgendaDto dto)
    {
        if (!await ValidarAgendaExistente(dto)) return OperationResult<Guid>.Failure(ValidationResult);

        var agenda = new Agenda(dto.MedicoId, dto.DataHora.ToUniversalTime());
        await _agendaRepository.Criar(agenda);
        await PersistData(_agendaRepository.UnitOfWork);
        return OperationResult<Guid>.Success(agenda.Id);
    }

    private async Task<bool> ValidarAgendaExistente(CriarAgendaDto dto)
    {
        var result = await _agendaRepository.BuscarPorMedicoHora(dto.MedicoId, dto.DataHora);

        if (result is not null)
        {
            AddError("Já existe agenda cadastrada para essa data/hora");
            return false;
        }

        return true;
    }
}
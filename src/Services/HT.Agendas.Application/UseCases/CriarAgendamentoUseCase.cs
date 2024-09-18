using HT.Agendas.Application.DTOs.Requests;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Models;
using HT.Agendas.Domain.Repository;
using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;

namespace HT.Agendas.Application.UseCases;

public class CriarAgendamentoUseCase : CommonUseCase, ICriarAgendamentoUseCase
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public CriarAgendamentoUseCase(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CriarAgendamentoDto dto)
    {
        if (!await ValidarAgendamentoExistente(dto)) return OperationResult<Guid>.Failure(ValidationResult);

        var agendamento = new Agendamento(dto.AgendaId, dto.PacienteId);
        await _agendamentoRepository.Criar(agendamento);
        await PersistData(_agendamentoRepository.UnitOfWork);
        return OperationResult<Guid>.Success(agendamento.Id);
    }

    private async Task<bool> ValidarAgendamentoExistente(CriarAgendamentoDto dto)
    {
        var result = await _agendamentoRepository.BuscarPorAgenda(dto.AgendaId);

        if (result is not null)
        {
            AddError("Já existe agendamento efetuado para essa data/hora.");
            return false;
        }

        return true;
    }
}
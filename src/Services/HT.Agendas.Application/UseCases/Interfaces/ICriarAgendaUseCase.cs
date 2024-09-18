using HT.Agendas.Application.DTOs.Requests;
using HT.Core.Commons.Communication;

namespace HT.Agendas.Application.UseCases.Interfaces;

public interface ICriarAgendaUseCase
{
    Task<OperationResult<Guid>> Handle(CriarAgendaDto dto);
}
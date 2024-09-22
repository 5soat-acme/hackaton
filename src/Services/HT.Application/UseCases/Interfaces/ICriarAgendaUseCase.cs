using HT.Application.DTOs.Requests;
using HT.Core.Commons.Communication;

namespace HT.Application.UseCases.Interfaces;

public interface ICriarAgendaUseCase
{
    Task<OperationResult<Guid>> Handle(CriarAgendaDto dto);
}
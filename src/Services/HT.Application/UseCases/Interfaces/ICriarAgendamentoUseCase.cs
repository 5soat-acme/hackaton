using HT.Application.DTOs.Requests;
using HT.Core.Commons.Communication;

namespace HT.Application.UseCases.Interfaces;

public interface ICriarAgendamentoUseCase
{
    Task<OperationResult<Guid>> Handle(CriarAgendamentoDto dto);
}

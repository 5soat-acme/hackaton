using HT.Agendas.Application.DTOs.Requests;
using HT.Core.Commons.Communication;

namespace HT.Agendas.Application.UseCases.Interfaces;

public interface ICriarAgendamentoUseCase
{
    Task<OperationResult<Guid>> Handle(CriarAgendamentoDto dto);
}

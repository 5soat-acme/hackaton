using HT.Core.Commons.Communication;
using HT.Cadastros.Application.DTOs.Requests;

namespace HT.Cadastros.Application.UseCases.Interfaces;

public interface ICriarMedicoUseCase
{
    Task<OperationResult<Guid>> Handle(CriarMedicoDto dto);
}
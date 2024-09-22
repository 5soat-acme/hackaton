using HT.Core.Commons.Communication;
using HT.Application.DTOs.Requests;

namespace HT.Application.UseCases.Interfaces;

public interface ICriarMedicoUseCase
{
    Task<OperationResult<Guid>> Handle(CriarMedicoDto dto);
}
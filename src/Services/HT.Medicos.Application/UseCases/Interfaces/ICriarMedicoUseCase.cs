using HT.Core.Commons.Communication;
using HT.Medicos.Application.DTOs.Requests;

namespace HT.Medicos.Application.UseCases.Interfaces;

public interface ICriarMedicoUseCase
{
    Task<OperationResult<Guid>> Handle(CriarMedicoDto dto);
}
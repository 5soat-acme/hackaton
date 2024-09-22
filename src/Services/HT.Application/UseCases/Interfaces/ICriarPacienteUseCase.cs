using HT.Core.Commons.Communication;
using HT.Application.DTOs.Requests;

namespace HT.Application.UseCases.Interfaces;

public interface ICriarPacienteUseCase
{
    Task<OperationResult<Guid>> Handle(CriarPacienteDto dto);
}
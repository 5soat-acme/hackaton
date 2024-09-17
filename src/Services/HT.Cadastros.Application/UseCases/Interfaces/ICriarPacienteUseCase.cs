using HT.Core.Commons.Communication;
using HT.Cadastros.Application.DTOs.Requests;

namespace HT.Cadastros.Application.UseCases.Interfaces;

public interface ICriarPacienteUseCase
{
    Task<OperationResult<Guid>> Handle(CriarPacienteDto dto);
}
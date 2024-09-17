using HT.Core.Commons.Communication;
using HT.Pacientes.Application.DTOs.Requests;

namespace HT.Pacientes.Application.UseCases.Interfaces;

public interface ICriarPacienteUseCase
{
    Task<OperationResult<Guid>> Handle(CriarPacienteDto dto);
}
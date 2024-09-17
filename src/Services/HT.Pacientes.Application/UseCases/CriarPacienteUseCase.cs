using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using HT.Pacientes.Application.DTOs.Requests;
using HT.Pacientes.Application.UseCases.Interfaces;
using HT.Pacientes.Domain.Models;
using HT.Pacientes.Domain.Repository;
using HT.Core.Commons.ValueObjects;

namespace HT.Pacientes.Application.UseCases;

public class CriarPacienteUseCase : CommonUseCase, ICriarPacienteUseCase
{
    private readonly IPacienteRepository _pacienteRepository;

    public CriarPacienteUseCase(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CriarPacienteDto dto)
    {
        var paciente = new Paciente(dto.Nome, new Cpf(dto.Cpf), dto.Email, dto.Senha);
        _pacienteRepository.Criar(paciente);
        await PersistData(_pacienteRepository.UnitOfWork);
        return OperationResult<Guid>.Success(paciente.Id);
    }
}
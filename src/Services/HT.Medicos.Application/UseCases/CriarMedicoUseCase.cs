using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using HT.Core.Commons.ValueObjects;
using HT.Medicos.Application.DTOs.Requests;
using HT.Medicos.Application.UseCases.Interfaces;
using HT.Medicos.Domain.Models;
using HT.Medicos.Domain.Repository;

namespace HT.Medicos.Application.UseCases;

public class CriarMedicoUseCase : CommonUseCase, ICriarMedicoUseCase
{
    private readonly IMedicoRepository _medicoRepository;

    public CriarMedicoUseCase(IMedicoRepository medicoRepository)
    {
        _medicoRepository = medicoRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CriarMedicoDto dto)
    {
        var medico = new Medico(dto.Nome, new Cpf(dto.Cpf), dto.NroCrm, dto.Email, dto.Senha);
        _medicoRepository.Criar(medico);
        await PersistData(_medicoRepository.UnitOfWork);
        return OperationResult<Guid>.Success(medico.Id);
    }
}
using HT.Core.Commons.Communication;
using HT.Core.Commons.UseCases;
using HT.Core.Commons.ValueObjects;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using HT.Usuarios.Application.Services.Interfaces;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Core.Commons.Identity;
using System.ComponentModel.DataAnnotations;
using HT.Core.Commons.Utils;

namespace HT.Cadastros.Application.UseCases;

public class CriarMedicoUseCase : CommonUseCase, ICriarMedicoUseCase
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAcessoAppService _acessoAppService;

    public CriarMedicoUseCase(IMedicoRepository medicoRepository,
        IAcessoAppService acessoAppService)
    {
        _medicoRepository = medicoRepository;
        _acessoAppService = acessoAppService;
    }

    public async Task<OperationResult<Guid>> Handle(CriarMedicoDto dto)
    {
        if (dto.Senha != dto.ConfirmacaoSenha) throw new ValidationException("Senhas não correspondem");
        if (!await ValidarMedico(dto)) return OperationResult<Guid>.Failure(ValidationResult);

        var medico = new Medico(dto.Nome, new Cpf(dto.Cpf), dto.NroCrm, dto.Email);
        await _medicoRepository.Criar(medico);
        await PersistData(_medicoRepository.UnitOfWork);

        await _acessoAppService.CriarUsuario(new NovoUsuario
        {
            Email = dto.Email,
            Senha = dto.Senha,
            TipoAcesso = TipoAcesso.MEDICO,
            Id = medico.Id
        });

        return OperationResult<Guid>.Success(medico.Id);
    }

    private async Task<bool> ValidarMedico(CriarMedicoDto dto)
    {
        var paciente = await _medicoRepository.BuscarPorEmail(dto.Email);
        if (paciente is not null)
        {
            AddError("Já existe médico cadastrado com esse email");
            return false;
        }

        paciente = await _medicoRepository.BuscarPorCpf(dto.Cpf.SomenteNumeros());
        if (paciente is not null)
        {
            AddError("Já existe médico cadastrado com esse CPF");
            return false;
        }

        return true;
    }
}
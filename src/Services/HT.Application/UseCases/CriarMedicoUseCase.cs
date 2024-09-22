using HT.Core.Commons.Communication;
using HT.Core.Commons.ValueObjects;
using HT.Application.DTOs.Requests;
using HT.Application.UseCases.Interfaces;
using HT.Domain.Models;
using HT.Domain.Repository;
using HT.Core.Commons.Identity;
using System.ComponentModel.DataAnnotations;
using HT.Core.Commons.Utils;
using Microsoft.AspNetCore.Identity;
using HT.Application.Services.Interfaces;
using HT.Infra.Extensions;

namespace HT.Application.UseCases;

public class CriarMedicoUseCase : CadastroCommonUseCase, ICriarMedicoUseCase
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAcessoAppService _acessoAppService;

    public CriarMedicoUseCase(IMedicoRepository medicoRepository,
        IAcessoAppService acessoAppService,
        UserManager<ApplicationUser> userManager) : base(userManager)
    {
        _medicoRepository = medicoRepository;
        _acessoAppService = acessoAppService;
    }

    public async Task<OperationResult<Guid>> Handle(CriarMedicoDto dto)
    {
        if (dto.Senha != dto.ConfirmacaoSenha) throw new ValidationException("Senhas não correspondem");
        if (!await ValidarUsuario(dto.Email, dto.Cpf)) return OperationResult<Guid>.Failure(ValidationResult);

        var medico = new Medico(dto.Nome, new Cpf(dto.Cpf), dto.NroCrm, dto.Email);
        await _medicoRepository.Criar(medico);
        await PersistData(_medicoRepository.UnitOfWork);

        await _acessoAppService.CriarUsuario(new NovoUsuario
        {
            Email = dto.Email,
            Senha = dto.Senha,
            TipoAcesso = TipoAcesso.MEDICO,
            Cpf = dto.Cpf.SomenteNumeros(),
            Id = medico.Id
        });

        return OperationResult<Guid>.Success(medico.Id);
    }
}
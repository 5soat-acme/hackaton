using HT.Core.Commons.Communication;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using HT.Core.Commons.ValueObjects;
using HT.Usuarios.Application.Services.Interfaces;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Core.Commons.Identity;
using System.ComponentModel.DataAnnotations;
using HT.Core.Commons.Utils;
using HT.Usuarios.Infra.Extensions;
using Microsoft.AspNetCore.Identity;

namespace HT.Cadastros.Application.UseCases;

public class CriarPacienteUseCase : CadastroCommonUseCase, ICriarPacienteUseCase
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IAcessoAppService _acessoAppService;

    public CriarPacienteUseCase(IPacienteRepository pacienteRepository,
        IAcessoAppService acessoAppService,
        UserManager<ApplicationUser> userManager) : base(userManager)
    {
        _pacienteRepository = pacienteRepository;
        _acessoAppService = acessoAppService;
    }

    public async Task<OperationResult<Guid>> Handle(CriarPacienteDto dto)
    {
        if (dto.Senha != dto.ConfirmacaoSenha) throw new ValidationException("Senhas não correspondem");
        if (!await ValidarUsuario(dto.Email, dto.Cpf)) return OperationResult<Guid>.Failure(ValidationResult);

        var paciente = new Paciente(dto.Nome, new Cpf(dto.Cpf), dto.Email);
        await _pacienteRepository.Criar(paciente);
        await PersistData(_pacienteRepository.UnitOfWork);

        await _acessoAppService.CriarUsuario(new NovoUsuario
        {
            Email = dto.Email,
            Senha = dto.Senha,
            TipoAcesso = TipoAcesso.PACIENTE,
            Cpf = dto.Cpf.SomenteNumeros(),
            Id = paciente.Id
        });

        return OperationResult<Guid>.Success(paciente.Id);
    }
}
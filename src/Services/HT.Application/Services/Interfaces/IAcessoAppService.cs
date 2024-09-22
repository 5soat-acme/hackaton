using HT.Application.DTOs.Requests;
using HT.Application.DTOs.Responses;
using HT.Core.Commons.Communication;

namespace HT.Application.Services.Interfaces;

public interface IAcessoAppService
{
    Task<OperationResult<Guid>> CriarUsuario(NovoUsuario novoUsuario);
    Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso usuario);
}
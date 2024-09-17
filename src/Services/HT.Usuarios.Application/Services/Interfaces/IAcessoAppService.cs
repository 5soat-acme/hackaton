using HT.Core.Commons.Communication;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Usuarios.Application.DTOs.Responses;

namespace HT.Usuarios.Application.Services.Interfaces;

public interface IAcessoAppService
{
    Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(NovoUsuario novoUsuario);
    Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso usuario);
}
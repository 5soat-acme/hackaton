using HT.Core.Commons.Communication;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Usuarios.Application.DTOs.Responses;
using System.Threading.Tasks;

namespace HT.Usuarios.Application.Services.Interfaces;

public interface IAcessoAppService
{
    Task<OperationResult<Guid>> CriarUsuario(NovoUsuario novoUsuario);
    Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso usuario);
}
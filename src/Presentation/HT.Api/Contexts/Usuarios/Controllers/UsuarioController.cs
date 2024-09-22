using HT.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;
using HT.Usuarios.Application.Services.Interfaces;
using HT.Usuarios.Application.DTOs.Responses;
using HT.Usuarios.Application.DTOs.Requests;

namespace HT.Api.Contexts.Cadastros.Controllers;

[Route("api/usuarios")]
public class UsuarioController : CustomControllerBase
{
    private readonly IAcessoAppService _acessoAppService;

    public UsuarioController(IAcessoAppService acessoAppService)
    {
        _acessoAppService = acessoAppService;
    }

    /// <summary>
    ///     Gera token de acesso para utilizar o sistema
    /// </summary>
    /// <response code="200">Token gerado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost("acessar")]
    public async Task<IActionResult> Logar(UsuarioAcesso usuario)
    {
        var result = await _acessoAppService.Identificar(usuario);

        if (!result.IsValid) return Respond(result.GetErrorMessages());

        return Respond(result.Data);
    }
}
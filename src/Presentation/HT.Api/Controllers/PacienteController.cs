using HT.Core.Commons.Communication;
using HT.Application.DTOs.Requests;
using HT.Application.UseCases.Interfaces;
using HT.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HT.Core.Commons.Identity;

namespace HT.Api.Controllers;

[Authorize]
[Route("api/pacientes")]
public class PacienteController : CustomControllerBase
{
    private readonly ICriarPacienteUseCase _criarPacienteUseCase;

    public PacienteController(ICriarPacienteUseCase criarPacienteUseCase)
    {
        _criarPacienteUseCase = criarPacienteUseCase;
    }

    /// <summary>
    ///     Cadastra um paciente
    /// </summary>
    /// <response code="200">Paciente cadastrado.</response>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarPacienteDto paciente)
    {
        var result = await _criarPacienteUseCase.Handle(paciente);
        return Respond(result);
    }
}
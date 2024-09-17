using HT.Core.Commons.Communication;
using HT.Medicos.Application.DTOs.Requests;
using HT.Medicos.Application.UseCases.Interfaces;
using HT.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Contexts.Medicos.Controllers;

//[Authorize]
[Route("api/medicos")]
public class MedicoController : CustomControllerBase
{
    private readonly ICriarMedicoUseCase _criarMedicoUseCase;

    public MedicoController(ICriarMedicoUseCase criarMedicoUseCase)
    {
        _criarMedicoUseCase = criarMedicoUseCase;
    }

    /// <summary>
    ///     Cadastra um médico
    /// </summary>
    /// <response code="200">Médico cadastrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarMedicoDto medico)
    {
        var result = await _criarMedicoUseCase.Handle(medico);
        return Respond(result);
    }
}
using HT.Core.Commons.Communication;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HT.Cadastros.Application.DTOs.Responses;

namespace HT.Api.Contexts.Cadastros.Controllers;

[Authorize]
[Route("api/medicos")]
public class MedicoController : CustomControllerBase
{
    private readonly ICriarMedicoUseCase _criarMedicoUseCase;
    private readonly IBuscarMedicosUseCase _buscarMedicosUseCase;

    public MedicoController(ICriarMedicoUseCase criarMedicoUseCase, 
        IBuscarMedicosUseCase buscarMedicosUseCase)
    {
        _criarMedicoUseCase = criarMedicoUseCase;
        _buscarMedicosUseCase = buscarMedicosUseCase;
    }

    /// <summary>
    ///     Cadastra um médico
    /// </summary>
    /// <response code="200">Médico cadastrado.</response>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarMedicoDto medico)
    {
        var result = await _criarMedicoUseCase.Handle(medico);
        return Respond(result);
    }

    /// <summary>
    ///     Obtém lista de médicos
    /// </summary>
    /// <response code="200">Lista de médicos.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MedicoDto>))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Buscar()
    {
        var medicos = await _buscarMedicosUseCase.Buscar();
        return medicos is null || !medicos.Any() ? NotFound() : Respond(medicos);
    }
}
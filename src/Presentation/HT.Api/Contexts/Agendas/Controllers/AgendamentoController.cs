using HT.Agendas.Application.DTOs.Requests;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Models;
using HT.Core.Commons.Communication;
using HT.Core.Commons.Identity;
using HT.WebApi.Commons.Controllers;
using HT.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Contexts.Agendas.Controllers;

[Authorize(Policy = AccessPolicy.PACIENTEPOLICY)]
[Route("api/agendamentos")]
public class AgendamentoController : CustomControllerBase
{
    private readonly IUserApp _userApp;
    private readonly ICriarAgendamentoUseCase _criarAgendamentoUseCase;

    public AgendamentoController(IUserApp userApp,
        ICriarAgendamentoUseCase criarAgendamentoUseCase)
    {
        _userApp = userApp;
        _criarAgendamentoUseCase = criarAgendamentoUseCase;
    }

    /// <summary>
    ///     Cria agendamento para o paciente
    /// </summary>
    /// <response code="200">Agendamento criado</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarAgendamentoDto agendamento)
    {
        agendamento.PacienteId = _userApp.GetUserId()!.Value;
        var result = await _criarAgendamentoUseCase.Handle(agendamento);
        return Respond(result);
    }
}
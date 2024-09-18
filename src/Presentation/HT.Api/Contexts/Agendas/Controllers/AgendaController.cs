﻿using HT.Agendas.Application.DTOs.Requests;
using HT.Agendas.Application.DTOs.Responses;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Core.Commons.Communication;
using HT.Core.Commons.Identity;
using HT.WebApi.Commons.Controllers;
using HT.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Contexts.Agendas.Controllers;

[Route("api/agendas")]
public class AgendaController : CustomControllerBase
{
    private readonly IUserApp _userApp;
    private readonly ICriarAgendaUseCase _criarAgendaUseCase;
    private readonly IRemoverAgendaUseCase _removerAgendaUseCase;
    private readonly IConsultarAgendaUseCase _consultarAgendaUseCase;

    public AgendaController(IUserApp userApp,
        ICriarAgendaUseCase criarAgendaUseCase,
        IRemoverAgendaUseCase removerAgendaUseCase,
        IConsultarAgendaUseCase consultarAgendaUseCase)
    {
        _userApp = userApp;
        _criarAgendaUseCase = criarAgendaUseCase;
        _removerAgendaUseCase = removerAgendaUseCase;
        _consultarAgendaUseCase = consultarAgendaUseCase;
    }

    /// <summary>
    ///     Cria agenda para médico
    /// </summary>
    /// <response code="200">Agenda criada</response>
    [Authorize(Policy = AccessPolicy.MEDICOPOLICY)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarAgendaDto agenda)
    {
        agenda.MedicoId = _userApp.GetUserId()!.Value;
        var result = await _criarAgendaUseCase.Handle(agenda);
        return Respond(result);
    }

    /// <summary>
    ///     Remove uma agenda
    /// </summary>
    /// <response code="200">Agenda removida.</response>
    [Authorize(Policy = AccessPolicy.MEDICOPOLICY)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var result = await _removerAgendaUseCase.Handle(id);

        return Respond(result);
    }

    /// <summary>
    ///     Obtém agendas disponíveis
    /// </summary>
    /// <response code="200">Lista de agendas disponíveis</response>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AgendaDto>))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Obter([FromQuery] Guid medicoId)
    {
        var medicos = await _consultarAgendaUseCase.BuscarDisponivelPorMedico(medicoId);
        return medicos is null || !medicos.Any() ? NotFound() : Respond(medicos);
    }
}
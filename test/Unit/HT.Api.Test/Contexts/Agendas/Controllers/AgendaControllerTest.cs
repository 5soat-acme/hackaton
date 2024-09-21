using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Core.Commons.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.WebApi.Commons.Users;
using HT.Api.Contexts.Agendas.Controllers;
using HT.Agendas.Application.DTOs.Requests;
using FluentAssertions;
using HT.Agendas.Application.DTOs.Responses;

namespace HT.Api.Test.Contexts.Agendas.Controllers;

public class AgendaControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserApp> _userAppMock;
    private readonly Mock<ICriarAgendaUseCase> _criarAgendaUseCaseMock;
    private readonly Mock<IRemoverAgendaUseCase> _removerAgendaUseCaseMock;
    private readonly Mock<IConsultarAgendaUseCase> _consultarAgendaUseCaseMock;
    private readonly AgendaController _agendaController;

    public AgendaControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        _userAppMock = _fixture.Freeze<Mock<IUserApp>>();
        _criarAgendaUseCaseMock = _fixture.Freeze<Mock<ICriarAgendaUseCase>>();
        _removerAgendaUseCaseMock = _fixture.Freeze<Mock<IRemoverAgendaUseCase>>();
        _consultarAgendaUseCaseMock = _fixture.Freeze<Mock<IConsultarAgendaUseCase>>();
        _agendaController = _fixture.Create<AgendaController>();
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoCriarAgenda()
    {
        // Arrange
        var criarAgendaDto = _fixture.Create<CriarAgendaDto>();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        _criarAgendaUseCaseMock.Setup(x => x.Handle(criarAgendaDto)).ReturnsAsync(operationResult);
        _userAppMock.Setup(x => x.GetUserId()).Returns(Guid.NewGuid());

        // Act
        var resultado = await _agendaController.Criar(criarAgendaDto);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult);
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoRemoverAgenda()
    {
        // Arrange
        var agendaId = Guid.NewGuid();
        var operationResult = OperationResult<Guid>.Success(agendaId);

        _removerAgendaUseCaseMock.Setup(x => x.Handle(agendaId)).ReturnsAsync(operationResult);
        // Act
        var resultado = await _agendaController.Remover(agendaId);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult);
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoObterAgendaPorMedico()
    {
        // Arrange
        var agendas = _fixture.CreateMany<AgendaDto>(5).ToList();
        var medicoId = Guid.NewGuid();

        _consultarAgendaUseCaseMock.Setup(x => x.BuscarDisponivelPorMedico(medicoId)).ReturnsAsync(agendas);

        // Act
        var resultado = await _agendaController.BuscarDisponivelPorMedico(medicoId);

        // Assert
        _consultarAgendaUseCaseMock.Verify(x => x.BuscarDisponivelPorMedico(medicoId), Times.Once);
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(agendas);
    }

    [Fact]
    public async Task DeveRetornarNotFound_QuandoNaoHouverMedicos()
    {
        // Arrange
        _consultarAgendaUseCaseMock.Setup(x => x.BuscarDisponivelPorMedico(It.IsAny<Guid>()))
            .ReturnsAsync((IEnumerable<AgendaDto>?)null);

        // Act
        var result = await _agendaController.BuscarDisponivelPorMedico(It.IsAny<Guid>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}

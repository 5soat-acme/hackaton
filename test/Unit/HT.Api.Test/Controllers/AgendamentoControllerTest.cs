using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Core.Commons.Communication;
using HT.WebApi.Commons.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using HT.Api.Controllers;
using HT.Application.UseCases.Interfaces;
using HT.Application.DTOs.Requests;

namespace HT.Api.Test.Controllers;

public class AgendamentoControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserApp> _userAppMock;
    private readonly Mock<ICriarAgendamentoUseCase> _criarAgendamentoUseCaseMock;
    private readonly AgendamentoController _agendamentoController;

    public AgendamentoControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        _userAppMock = _fixture.Freeze<Mock<IUserApp>>();
        _criarAgendamentoUseCaseMock = _fixture.Freeze<Mock<ICriarAgendamentoUseCase>>();
        _agendamentoController = _fixture.Create<AgendamentoController>();
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoCriarAgendamento()
    {
        // Arrange
        var criarAgendamentoDto = _fixture.Create<CriarAgendamentoDto>();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        _criarAgendamentoUseCaseMock.Setup(x => x.Handle(criarAgendamentoDto)).ReturnsAsync(operationResult);
        _userAppMock.Setup(x => x.GetUserId()).Returns(Guid.NewGuid());

        // Act
        var resultado = await _agendamentoController.Criar(criarAgendamentoDto);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult);
    }
}

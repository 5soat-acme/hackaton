using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Api.Contexts.Cadastros.Controllers;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Core.Commons.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace HT.Api.Test.Contexts.Cadastros.Controllers;

public class PacienteControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<ICriarPacienteUseCase> _criarPacienteUseCaseMock;
    private readonly PacienteController _pacienteController;

    public PacienteControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        _criarPacienteUseCaseMock = _fixture.Freeze<Mock<ICriarPacienteUseCase>>();
        _pacienteController = _fixture.Create<PacienteController>();
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoCriarPaciente()
    {
        // Arrange
        var criarPacienteDto = _fixture.Create<CriarPacienteDto>();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        _criarPacienteUseCaseMock.Setup(x => x.Handle(criarPacienteDto)).ReturnsAsync(operationResult);
        // Act
        var resultado = await _pacienteController.Criar(criarPacienteDto);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult);
    }
}

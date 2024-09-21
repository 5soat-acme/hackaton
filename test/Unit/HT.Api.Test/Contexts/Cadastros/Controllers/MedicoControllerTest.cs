using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Core.Commons.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Api.Contexts.Cadastros.Controllers;
using FluentAssertions;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.DTOs.Responses;

namespace HT.Api.Test.Contexts.Cadastros.Controllers;

public class MedicoControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<ICriarMedicoUseCase> _criarMedicoUseCaseMock;
    private readonly Mock<IBuscarMedicosUseCase> _buscarMedicosUseCaseMock;
    private readonly MedicoController _medicoController;

    public MedicoControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        _criarMedicoUseCaseMock = _fixture.Freeze<Mock<ICriarMedicoUseCase>>();
        _buscarMedicosUseCaseMock = _fixture.Freeze<Mock<IBuscarMedicosUseCase>>();
        _medicoController = _fixture.Create<MedicoController>();
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoCriarMedico()
    {
        // Arrange
        var criarMedicoDto = _fixture.Create<CriarMedicoDto>();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        _criarMedicoUseCaseMock.Setup(x => x.Handle(criarMedicoDto)).ReturnsAsync(operationResult);
        // Act
        var resultado = await _medicoController.Criar(criarMedicoDto);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult);
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoObterMedicos()
    {
        // Arrange
        var medicos = _fixture.CreateMany<MedicoDto>(5).ToList();

        _buscarMedicosUseCaseMock.Setup(x => x.Buscar()).ReturnsAsync(medicos);

        // Act
        var resultado = await _medicoController.Buscar();

        // Assert
        _buscarMedicosUseCaseMock.Verify(x => x.Buscar(), Times.Once);
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(medicos);
    }

    [Fact]
    public async Task DeveRetornarNotFound_QuandoNaoHouverMedicos()
    {
        // Arrange
        _buscarMedicosUseCaseMock.Setup(x => x.Buscar())
            .ReturnsAsync((IEnumerable<MedicoDto>?)null);

        // Act
        var result = await _medicoController.Buscar();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}

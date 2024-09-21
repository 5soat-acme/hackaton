using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Api.Contexts.Cadastros.Controllers;
using HT.Core.Commons.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using HT.Usuarios.Application.Services.Interfaces;
using FluentAssertions;
using HT.Usuarios.Application.DTOs.Responses;
using HT.Usuarios.Application.DTOs.Requests;

namespace HT.Api.Test.Contexts.Usuarios.Controllers;

public class UsuarioControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IAcessoAppService> _acessoAppServiceMock;
    private readonly UsuarioController _usuarioController;

    public UsuarioControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        _acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        _usuarioController = _fixture.Create<UsuarioController>();
    }

    [Fact]
    public async Task DeveRetornarOk_QuandoAcessarLogin()
    {
        // Arrange
        var usuario = _fixture.Create<UsuarioAcesso>();
        var operationResult = OperationResult<RespostaTokenAcesso>.Success(_fixture.Create<RespostaTokenAcesso>());
        _acessoAppServiceMock.Setup(x => x.Identificar(usuario)).ReturnsAsync(operationResult);

        // Act
        var resultado = await _usuarioController.Logar(usuario);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(operationResult.Data);
    }

    [Fact]
    public async Task DeveRetornarBadRequest_QuandoFalharAoLogar()
    {
        // Arrange
        var usuario = _fixture.Create<UsuarioAcesso>();
        var operationResult = OperationResult<RespostaTokenAcesso>.Failure("Erro");
        _acessoAppServiceMock.Setup(x => x.Identificar(usuario)).ReturnsAsync(operationResult);

        // Act
        var resultado = await _usuarioController.Logar(usuario);

        // Assert
        var badRequestResult = resultado as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().BeEquivalentTo(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", operationResult.GetErrorMessages().ToArray() }
        }));
    }
}

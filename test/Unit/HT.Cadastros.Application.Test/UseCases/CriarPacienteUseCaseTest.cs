using AutoFixture.AutoMoq;
using AutoFixture;
using HT.Cadastros.Application.DTOs.Requests;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Domain.Repository;
using HT.Core.Commons.Communication;
using HT.Usuarios.Application.Services.Interfaces;
using HT.Usuarios.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Moq;
using HT.Cadastros.Application.UseCases;
using HT.Cadastros.Domain.Models;
using HT.Usuarios.Application.DTOs.Requests;
using System.Linq.Expressions;
using FluentAssertions;

namespace HT.Cadastros.Application.Test.UseCases;

public class CriarPacienteUseCaseTest
{
    private readonly IFixture _fixture;

    public CriarPacienteUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var pacienteRepositoryMock = _fixture.Freeze<Mock<IPacienteRepository>>();
        var acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        var userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
        _fixture.Register<ICriarPacienteUseCase>(() => new CriarPacienteUseCase(pacienteRepositoryMock.Object,
                                                                            acessoAppServiceMock.Object,
                                                                            userManagerMock.Object));
    }

    [Fact]
    public async Task DeveCriarPaciente()
    {
        // Arrange
        var pacienteDto = _fixture.Build<CriarPacienteDto>()
            .With(x => x.Senha, "123")
            .With(x => x.ConfirmacaoSenha, "123")
            .With(x => x.Cpf, "27997712038")
            .Create();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        var users = new List<ApplicationUser>().AsQueryable();

        var pacienteRepositoryMock = _fixture.Freeze<Mock<IPacienteRepository>>();
        var acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        var userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();

        pacienteRepositoryMock.Setup(x => x.Criar(It.IsAny<Paciente>())).Returns(Task.CompletedTask);
        acessoAppServiceMock.Setup(x => x.CriarUsuario(It.IsAny<NovoUsuario>())).ReturnsAsync(operationResult);
        var mockDbSet = new Mock<DbSet<ApplicationUser>>();
        var asyncQueryProvider = new Mock<IAsyncQueryProvider>();
        asyncQueryProvider.Setup(x => x.ExecuteAsync<ApplicationUser>(It.IsAny<Expression>(), It.IsAny<CancellationToken>()))
                          .Returns(users.FirstOrDefault());
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(asyncQueryProvider.Object);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
        userManagerMock.Setup(x => x.FindByEmailAsync(pacienteDto.Email)).ReturnsAsync((ApplicationUser?)null);
        userManagerMock.Setup(x => x.Users).Returns(mockDbSet.Object);

        var useCase = _fixture.Create<ICriarPacienteUseCase>();

        // Act
        var resultado = await useCase.Handle(pacienteDto);

        // Assert
        pacienteRepositoryMock.Verify(x => x.Criar(It.IsAny<Paciente>()), Times.Once);
        acessoAppServiceMock.Verify(x => x.CriarUsuario(It.IsAny<NovoUsuario>()), Times.Once);
        pacienteRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
        resultado.IsValid.Should().BeTrue();
        resultado.ValidationResult.IsValid.Should().BeTrue();
    }
}

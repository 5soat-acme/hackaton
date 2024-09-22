using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using HT.Domain.Repository;
using HT.Application.UseCases.Interfaces;
using HT.Application.UseCases;
using Microsoft.AspNetCore.Identity;
using HT.Application.DTOs.Requests;
using HT.Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using HT.Core.Commons.Communication;
using HT.Application.Services.Interfaces;
using HT.Infra.Extensions;

namespace HT.Application.Test.UseCases;

public class CriarMedicoUseCaseTest
{
    private readonly IFixture _fixture;

    public CriarMedicoUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        var acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        var userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
        _fixture.Register<ICriarMedicoUseCase>(() => new CriarMedicoUseCase(medicoRepositoryMock.Object, 
                                                                            acessoAppServiceMock.Object, 
                                                                            userManagerMock.Object));
    }

    [Fact]
    public async Task DeveCriarMedico()
    {
        // Arrange
        var medicoDto = _fixture.Build<CriarMedicoDto>()
            .With(x => x.Senha, "123")
            .With(x => x.ConfirmacaoSenha, "123")
            .With(x => x.Cpf, "27997712038")
            .Create();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        var users = new List<ApplicationUser>().AsQueryable();

        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        var acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        var userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();

        medicoRepositoryMock.Setup(x => x.Criar(It.IsAny<Medico>())).Returns(Task.CompletedTask);
        acessoAppServiceMock.Setup(x => x.CriarUsuario(It.IsAny<NovoUsuario>())).ReturnsAsync(operationResult);
        var mockDbSet = new Mock<DbSet<ApplicationUser>>();
        var asyncQueryProvider = new Mock<IAsyncQueryProvider>();
        asyncQueryProvider.Setup(x => x.ExecuteAsync<ApplicationUser>(It.IsAny<Expression>(), It.IsAny<CancellationToken>()))
                          .Returns(users.FirstOrDefault());
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(asyncQueryProvider.Object);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
        userManagerMock.Setup(x => x.FindByEmailAsync(medicoDto.Email)).ReturnsAsync((ApplicationUser?) null);
        userManagerMock.Setup(x => x.Users).Returns(mockDbSet.Object);

        var useCase = _fixture.Create<ICriarMedicoUseCase>();

        // Act
        var resultado = await useCase.Handle(medicoDto);

        // Assert
        medicoRepositoryMock.Verify(x => x.Criar(It.IsAny<Medico>()), Times.Once);
        acessoAppServiceMock.Verify(x => x.CriarUsuario(It.IsAny<NovoUsuario>()), Times.Once);
        medicoRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
        resultado.IsValid.Should().BeTrue();
        resultado.ValidationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeveRetornarErro_QuandoCriarMedicoComEmailJaExistente()
    {
        // Arrange
        var medicoDto = _fixture.Build<CriarMedicoDto>()
            .With(x => x.Senha, "123")
            .With(x => x.ConfirmacaoSenha, "123")
            .With(x => x.Cpf, "27997712038")
            .Create();
        var operationResult = OperationResult<Guid>.Success(Guid.NewGuid());

        var users = new List<ApplicationUser>().AsQueryable();
        var applicationUser = _fixture.Create<ApplicationUser>();

        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        var acessoAppServiceMock = _fixture.Freeze<Mock<IAcessoAppService>>();
        var userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();        
        userManagerMock.Setup(x => x.FindByEmailAsync(medicoDto.Email)).ReturnsAsync(applicationUser);

        var useCase = _fixture.Create<ICriarMedicoUseCase>();

        // Act
        var resultado = await useCase.Handle(medicoDto);

        // Assert
        medicoRepositoryMock.Verify(x => x.Criar(It.IsAny<Medico>()), Times.Never);
        acessoAppServiceMock.Verify(x => x.CriarUsuario(It.IsAny<NovoUsuario>()), Times.Never);
        medicoRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Never);
        resultado.IsValid.Should().BeFalse();
        resultado.GetErrorMessages().Count(x => x == "Já existe cadastrado com esse email").Should().Be(1);
    }
}

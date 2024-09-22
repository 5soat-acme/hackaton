using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using FluentAssertions;
using System.Globalization;
using HT.Domain.Repository;
using HT.Application.UseCases;
using HT.Application.UseCases.Interfaces;
using HT.Application.DTOs.Requests;
using HT.Domain.Models;

namespace HT.Application.Test.UseCases;

public class CriarAgendaUseCaseTest
{
    private readonly IFixture _fixture;

    public CriarAgendaUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        _fixture.Register<ICriarAgendaUseCase>(() => new CriarAgendaUseCase(agendaRepositoryMock.Object));
    }

    [Fact]
    public async Task DeveCriarAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agendaDto = new CriarAgendaDto
        {
            MedicoId = Guid.NewGuid(),
            DataHora = data
        };

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.Criar(It.IsAny<Agenda>())).Returns(Task.CompletedTask);
        agendaRepositoryMock.Setup(x => x.BuscarPorMedicoHora(agendaDto.MedicoId, agendaDto.DataHora)).ReturnsAsync((Agenda?)null);

        var useCase = _fixture.Create<ICriarAgendaUseCase>();

        // Act
        var resultado = await useCase.Handle(agendaDto);

        // Assert
        agendaRepositoryMock.Verify(x => x.Criar(It.IsAny<Agenda>()), Times.Once);
        agendaRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
        resultado.IsValid.Should().BeTrue();
        resultado.ValidationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeveRetornarErro_QuandoCriarAgendaJaExistente()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agendaDto = new CriarAgendaDto
        {
            MedicoId = Guid.NewGuid(),
            DataHora = data
        };
        var agenda = new Agenda(agendaDto.MedicoId, agendaDto.DataHora);

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.Criar(It.IsAny<Agenda>())).Returns(Task.CompletedTask);
        agendaRepositoryMock.Setup(x => x.BuscarPorMedicoHora(agendaDto.MedicoId, agendaDto.DataHora)).ReturnsAsync(agenda);

        var useCase = _fixture.Create<ICriarAgendaUseCase>();

        // Act
        var resultado = await useCase.Handle(agendaDto);

        // Assert
        agendaRepositoryMock.Verify(x => x.Criar(It.IsAny<Agenda>()), Times.Never);
        agendaRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Never);
        resultado.IsValid.Should().BeFalse();
        resultado.GetErrorMessages().Count(x => x == "Já existe agenda cadastrada para essa data/hora").Should().Be(1);
    }
}

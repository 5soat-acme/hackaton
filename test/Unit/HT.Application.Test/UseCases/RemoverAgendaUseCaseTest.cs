using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using Moq;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using HT.Domain.Repository;
using HT.Application.UseCases;
using HT.Application.UseCases.Interfaces;
using HT.Domain.Models;

namespace HT.Application.Test.UseCases;

public class RemoverAgendaUseCaseTest
{
    private readonly IFixture _fixture;

    public RemoverAgendaUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        _fixture.Register<IRemoverAgendaUseCase>(() => new RemoverAgendaUseCase(agendaRepositoryMock.Object));
    }

    [Fact]
    public async Task DeveRemoverAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.BuscarPorIdEMedicoId(agenda.Id, agenda.MedicoId)).ReturnsAsync(agenda);

        var useCase = _fixture.Create<IRemoverAgendaUseCase>();

        // Act
        var resultado = await useCase.Handle(agenda.Id, agenda.MedicoId);

        // Assert
        agendaRepositoryMock.Verify(x => x.BuscarPorIdEMedicoId(agenda.Id, agenda.MedicoId), Times.Once);
        agendaRepositoryMock.Verify(x => x.Remover(It.IsAny<Agenda>()), Times.Once);
        agendaRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
        resultado.IsValid.Should().BeTrue();
        resultado.ValidationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeveGerarExcecao_QuandoRemoverAgendaInexistente()
    {
        // Arrange
        var agendaId = _fixture.Create<Guid>();
        var medicoId = _fixture.Create<Guid>();

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.BuscarPorIdEMedicoId(agendaId, medicoId)).ReturnsAsync((Agenda?)null);

        var useCase = _fixture.Create<IRemoverAgendaUseCase>();

        // Act
        Func<Task> act = async () => await useCase.Handle(agendaId, medicoId);

        // Assert
        agendaRepositoryMock.Verify(x => x.BuscarPorIdEMedicoId(agendaId, medicoId), Times.Never);
        agendaRepositoryMock.Verify(x => x.Remover(It.IsAny<Agenda>()), Times.Never);
        agendaRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Never);
        await act.Should().ThrowAsync<ValidationException>().WithMessage("Agenda não existe");
    }
}


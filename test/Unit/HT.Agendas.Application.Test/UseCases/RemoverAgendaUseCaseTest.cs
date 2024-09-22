using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Repository;
using HT.Agendas.Application.UseCases;
using HT.Agendas.Application.DTOs.Requests;
using System.Globalization;
using HT.Agendas.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HT.Agendas.Application.Test.UseCases;

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
        agendaRepositoryMock.Setup(x => x.BuscarPorId(agenda.Id)).ReturnsAsync(agenda);

        var useCase = _fixture.Create<IRemoverAgendaUseCase>();

        // Act
        var resultado = await useCase.Handle(agenda.Id);

        // Assert
        agendaRepositoryMock.Verify(x => x.BuscarPorId(agenda.Id), Times.Once);
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

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.BuscarPorId(agendaId)).ReturnsAsync((Agenda?)null);

        var useCase = _fixture.Create<IRemoverAgendaUseCase>();

        // Act
        Func<Task> act = async () => await useCase.Handle(agendaId);

        // Assert
        agendaRepositoryMock.Verify(x => x.BuscarPorId(agendaId), Times.Never);
        agendaRepositoryMock.Verify(x => x.Remover(It.IsAny<Agenda>()), Times.Never);
        agendaRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Never);
        await act.Should().ThrowAsync<ValidationException>().WithMessage("Agenda não existe");
    }
}


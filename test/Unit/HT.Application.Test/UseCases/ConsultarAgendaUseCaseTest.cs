using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using FluentAssertions;
using System.Globalization;
using HT.Application.UseCases.Interfaces;
using HT.Application.UseCases;
using HT.Domain.Repository;
using HT.Domain.Models;
using HT.Application.DTOs.Responses;

namespace HT.Application.Test.UseCases;

public class ConsultarAgendaUseCaseTest
{
    private readonly IFixture _fixture;

    public ConsultarAgendaUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        _fixture.Register<IConsultarAgendaUseCase>(() => new ConsultarAgendaUseCase(agendaRepositoryMock.Object));
    }

    [Fact]
    public async Task DeveConsultarAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var listaAgendas = new List<Agenda>() { agenda };
        var listaAgendasEsperadas = new List<AgendaDto>() { new AgendaDto()
        {
            Id = agenda.Id,
            DataHora = agenda.DataHora,
        }};

        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        agendaRepositoryMock.Setup(x => x.BuscarDisponivelPorMedico(agenda.MedicoId)).ReturnsAsync(listaAgendas);

        var useCase = _fixture.Create<IConsultarAgendaUseCase>();

        // Act
        var resultado = await useCase.BuscarDisponivelPorMedico(agenda.MedicoId);

        // Assert
        agendaRepositoryMock.Verify(x => x.BuscarDisponivelPorMedico(agenda.MedicoId), Times.Once);
        resultado.Should().BeEquivalentTo(listaAgendasEsperadas);
    }
}

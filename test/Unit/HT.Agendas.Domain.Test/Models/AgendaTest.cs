using AutoFixture;
using FluentAssertions;
using HT.Agendas.Domain.Models;
using HT.Core.Commons.DomainObjects;
using System.Globalization;

namespace HT.Agendas.Domain.Test.Models;

public class AgendaTest
{
    [Fact]
    public void DeveCriarUmaInstanciaDeAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);

        // Act - Assert
        agenda.Should().BeOfType<Agenda>();
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarAgendaComMedicoInvalido()
    {
        // Arrange - Act
        Action act = () => new Agenda(Guid.Empty, DateTime.Now);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Médico inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarAgendaComDataInvalida()
    {
        // Arrange - Act
        string dataString = "21/09/2024 20:34:45";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        Action act = () => new Agenda(Guid.NewGuid(), data);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Data/Hora inválida");
    }
}

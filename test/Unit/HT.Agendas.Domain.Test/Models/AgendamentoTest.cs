using FluentAssertions;
using HT.Agendas.Domain.Models;
using HT.Core.Commons.DomainObjects;

namespace HT.Agendamentos.Domain.Test.Models;

public class AgendamentomentoTest
{
    [Fact]
    public void DeveCriarUmaInstanciaDeAgendamento()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid());

        // Act - Assert
        agendamento.Should().BeOfType<Agendamento>();
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarAgendamentoComAgendaInvalida()
    {
        // Arrange - Act
        Action act = () => new Agendamento(Guid.Empty, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Agenda inválida");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarAgendamentoComPacienteInvalido()
    {
        // Arrange - Act
        Action act = () => new Agendamento(Guid.NewGuid(), Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Paciente inválido");
    }
}
using FluentAssertions;
using HT.Domain.Models;
using HT.Domain.Test.Fixtures;
using HT.Core.Commons.DomainObjects;
using HT.Core.Commons.ValueObjects;

namespace HT.Domain.Test.Models;

[Collection(nameof(PacienteCollection))]
public class PacienteTest(PacienteFixture fixture)
{
    [Fact]
    public void DeveCriarUmaInstanciaDePaciente()
    {
        // Arrange
        var paciente = fixture.GerarPaciente();

        // Act - Assert
        paciente.Should().BeOfType<Paciente>();
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarPacienteComNomeInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarPaciente(nome: "");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Nome inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarPacienteComCpfInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarPaciente(cpf: new Cpf(""));
        Action act2 = () => new Paciente("Paciente 1", null, "email@example.com");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("CPF inválido");
        act2.Should().Throw<DomainException>().WithMessage("CPF inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarPacienteComEmailInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarPaciente(email: "");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Email inválido");
    }
}

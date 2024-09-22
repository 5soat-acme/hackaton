using FluentAssertions;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Test.Fixtures;
using HT.Core.Commons.DomainObjects;
using HT.Core.Commons.ValueObjects;

namespace HT.Cadastros.Domain.Test.Models;

[Collection(nameof(MedicoCollection))]
public class MedicoTest(MedicoFixture fixture)
{
    [Fact]
    public void DeveCriarUmaInstanciaDeMedico()
    {
        // Arrange
        var medico= fixture.GerarMedico();

        // Act - Assert
        medico.Should().BeOfType<Medico>();
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarMedicoComNomeInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarMedico(nome: "");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Nome inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarMedicoComCpfInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarMedico(cpf: new Cpf(""));
        Action act2 = () => new Medico("Medico 1", null, "123", "email@example.com");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("CPF inválido");
        act2.Should().Throw<DomainException>().WithMessage("CPF inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarMedicoComNroCrmInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarMedico(nroCrm: "");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Número CRM inválido");
    }

    [Fact]
    public void DeveGerarExcecao_QuandoCriarMedicoComEmailInvalido()
    {
        // Arrange - Act
        Action act = () => fixture.GerarMedico(email: "");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Email inválido");
    }
}

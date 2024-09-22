using Bogus;
using HT.Domain.Models;
using HT.Core.Commons.ValueObjects;

namespace HT.Domain.Test.Fixtures;
[CollectionDefinition(nameof(PacienteCollection))]
public class PacienteCollection : ICollectionFixture<PacienteFixture>
{
}

public class PacienteFixture
{
    public Paciente GerarPaciente(string? nome = null, Cpf? cpf = null, string? email = null)
    {
        var paciente = new Faker<Paciente>("pt_BR")
            .CustomInstantiator(f => new Paciente(nome ?? f.Commerce.ProductName(), cpf ?? new Cpf("27997712038"), email ?? f.Person.Email));

        return paciente.Generate();
    }
}
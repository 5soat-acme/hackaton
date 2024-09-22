using Bogus;
using HT.Cadastros.Domain.Models;
using HT.Core.Commons.ValueObjects;

namespace HT.Cadastros.Domain.Test.Fixtures;
[CollectionDefinition(nameof(MedicoCollection))]
public class MedicoCollection : ICollectionFixture<MedicoFixture>
{
}

public class MedicoFixture
{
    public Medico GerarMedico(string? nome = null, Cpf? cpf = null, string? nroCrm = null, string? email = null)
    {
        var medico = new Faker<Medico>("pt_BR")
            .CustomInstantiator(f => new Medico(nome ?? f.Commerce.ProductName(), cpf ?? new Cpf("27997712038"),
            nroCrm ?? f.Random.Int(1, 20).ToString(), email ?? f.Person.Email));

        return medico.Generate();
    }
}
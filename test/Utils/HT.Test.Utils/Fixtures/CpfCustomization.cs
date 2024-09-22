using AutoFixture;
using HT.Core.Commons.ValueObjects;

namespace HT.Test.Utils.Fixtures;

public class CpfCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Cpf>(c => c.FromFactory(() => new Cpf("42208604016")));
    }
}
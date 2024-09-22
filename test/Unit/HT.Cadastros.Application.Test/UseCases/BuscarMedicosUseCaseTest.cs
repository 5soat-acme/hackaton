using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using HT.Cadastros.Domain.Repository;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Application.UseCases;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Application.DTOs.Responses;
using FluentAssertions;
using HT.Test.Utils.Fixtures;

namespace HT.Cadastros.Application.Test.UseCases;

public class BuscarMedicosUseCaseTest
{
    private readonly IFixture _fixture;

    public BuscarMedicosUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize(new CpfCustomization());
        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        _fixture.Register<IBuscarMedicosUseCase>(() => new BuscarMedicosUseCase(medicoRepositoryMock.Object));
    }

    [Fact]
    public async Task DeveBuscarMedicos()
    {
        // Arrange
        var medico = _fixture.Create<Medico>();
        var listaMedicos = new List<Medico>() { medico };
        var listaMedicosEsperados = new List<MedicoDto>() { new MedicoDto()
        {
            Id = medico.Id,
            Nome = medico.Nome,
            Email = medico.Email
        }};

        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        medicoRepositoryMock.Setup(x => x.Buscar()).ReturnsAsync(listaMedicos);

        var useCase = _fixture.Create<IBuscarMedicosUseCase>();

        // Act
        var resultado = await useCase.Buscar();

        // Assert
        medicoRepositoryMock.Verify(x => x.Buscar(), Times.Once);
        resultado.Should().BeEquivalentTo(listaMedicosEsperados);
    }
}

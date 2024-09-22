using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using HT.Cadastros.Infra.Data;
using HT.Cadastros.Infra.Data.Repository;
using HT.Test.Utils.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HT.Cadastros.Infra.Test.Data.Repository;

public class MedicoRepositoryTest : IDisposable
{
    private readonly CadastroDbContext _context;
    private readonly IFixture _fixture;
    private bool disposed = false;

    public MedicoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<CadastroDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

        _context = new CadastroDbContext(options);

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize(new CpfCustomization());
        _fixture.Register<IMedicoRepository>(() => new MedicoRepository(_context));
    }

    [Fact]
    public async Task DeveCriarUmMedico()
    {
        // Arrange
        var medico = _fixture.Create<Medico>();
        var repository = _fixture.Create<IMedicoRepository>();

        // Act
        await repository.Criar(medico);
        bool commit = await _context.Commit();

        // Assert
        commit.Should().BeTrue();
        _context.Medicos.Should().Contain(medico);
        var medicoSalvo = await _context.Medicos.FindAsync(medico.Id);
        medicoSalvo.Should().NotBeNull();
        medicoSalvo.Should().BeEquivalentTo(medico);
        repository.UnitOfWork.Should().Be(_context);
    }

    [Fact]
    public async Task DeveBuscarMedicos()
    {
        // Arrange
        var medicos = _fixture.CreateMany<Medico>(5);

        var repository = _fixture.Create<IMedicoRepository>();
        foreach (var medico in medicos)
        {
            await repository.Criar(medico);
        }
        await _context.Commit();

        // Act
        var result = await repository.Buscar();

        // Assert
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task DeveBuscarMedicoPorId()
    {
        // Arrange
        var medico = _fixture.Create<Medico>();
        var repository = _fixture.Create<IMedicoRepository>();
        await repository.Criar(medico);
        await _context.Commit();

        // Act
        var result = await repository.BuscarPorId(medico.Id);

        // Assert
        result.Should().BeEquivalentTo(medico);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
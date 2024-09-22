using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using HT.Cadastros.Domain.Models;
using HT.Cadastros.Domain.Repository;
using HT.Cadastros.Infra.Data;
using HT.Test.Utils.Fixtures;
using Microsoft.EntityFrameworkCore;
using HT.Cadastros.Infra.Data.Repository;

namespace HT.Cadastros.Infra.Test.Data.Repository;

public class PacienteRepositoryTest : IDisposable
{
    private readonly CadastroDbContext _context;
    private readonly IFixture _fixture;
    private bool disposed = false;

    public PacienteRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<CadastroDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

        _context = new CadastroDbContext(options);

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize(new CpfCustomization());
        _fixture.Register<IPacienteRepository>(() => new PacienteRepository(_context));
    }

    [Fact]
    public async Task DeveCriarUmPaciente()
    {
        // Arrange
        var paciente = _fixture.Create<Paciente>();
        var repository = _fixture.Create<IPacienteRepository>();

        // Act
        await repository.Criar(paciente);
        bool commit = await _context.Commit();

        // Assert
        commit.Should().BeTrue();
        _context.Pacientes.Should().Contain(paciente);
        var pacienteSalvo = await _context.Pacientes.FindAsync(paciente.Id);
        pacienteSalvo.Should().NotBeNull();
        pacienteSalvo.Should().BeEquivalentTo(paciente);
        repository.UnitOfWork.Should().Be(_context);
    }

    [Fact]
    public async Task DeveBuscarPacientes()
    {
        // Arrange
        var pacientes = _fixture.CreateMany<Paciente>(5);

        var repository = _fixture.Create<IPacienteRepository>();
        foreach (var paciente in pacientes)
        {
            await repository.Criar(paciente);
        }
        await _context.Commit();

        // Act
        var result = await repository.Buscar();

        // Assert
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task DeveBuscarPacientePorId()
    {
        // Arrange
        var paciente = _fixture.Create<Paciente>();
        var repository = _fixture.Create<IPacienteRepository>();
        await repository.Criar(paciente);
        await _context.Commit();

        // Act
        var result = await repository.BuscarPorId(paciente.Id);

        // Assert
        result.Should().BeEquivalentTo(paciente);
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

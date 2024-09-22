using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using HT.Test.Utils.Fixtures;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using HT.Infra.Data;
using HT.Domain.Repository;
using HT.Infra.Data.Repository;
using HT.Domain.Models;

namespace HT.Infra.Test.Data.Repository;

public class AgendaRepositoryTest : IDisposable
{
    private readonly HackatonDbContext _context;
    private readonly IFixture _fixture;
    private bool disposed = false;

    public AgendaRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<HackatonDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDbAgenda")
            .Options;

        _context = new HackatonDbContext(options);

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize(new CpfCustomization());
        _fixture.Register<IAgendaRepository>(() => new AgendaRepository(_context));
    }

    [Fact]
    public async Task DeveCriarUmaAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var repository = _fixture.Create<IAgendaRepository>();

        // Act
        await repository.Criar(agenda);
        bool commit = await _context.Commit();

        // Assert
        commit.Should().BeTrue();
        _context.Agendas.Should().Contain(agenda);
        var medicoSalvo = await _context.Agendas.FindAsync(agenda.Id);
        medicoSalvo.Should().NotBeNull();
        medicoSalvo.Should().BeEquivalentTo(agenda);
        repository.UnitOfWork.Should().Be(_context);
    }

    [Fact]
    public async Task DeveRemoverAgenda()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var repository = _fixture.Create<IAgendaRepository>();
        await repository.Criar(agenda);
        await _context.Commit();
        _context.Entry(agenda).State = EntityState.Detached;

        // Act
        var prodRemover = await repository.BuscarPorId(agenda.Id);
        repository.Remover(prodRemover!);
        bool commit = await _context.Commit();

        // Assert
        commit.Should().BeTrue();
        _context.Agendas.Should().HaveCount(0);
    }

    [Fact]
    public async Task DeveBuscarAgendaPorId()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var repository = _fixture.Create<IAgendaRepository>();
        await repository.Criar(agenda);
        await _context.Commit();

        // Act
        var result = await repository.BuscarPorId(agenda.Id);

        // Assert
        result.Should().BeEquivalentTo(agenda);
    }

    [Fact]
    public async Task DeveBuscarAgendasDisponiveisPorMedicoPorId()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var listaAgendas = new List<Agenda> { agenda };
        var repository = _fixture.Create<IAgendaRepository>();
        await repository.Criar(agenda);
        await _context.Commit();

        // Act
        var result = await repository.BuscarDisponivelPorMedico(agenda.MedicoId);

        // Assert
        result.Should().BeEquivalentTo(listaAgendas);
    }

    [Fact]
    public async Task DeveBuscarAgendaPorMedicoEHora()
    {
        // Arrange
        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var agenda = new Agenda(Guid.NewGuid(), data);
        var repository = _fixture.Create<IAgendaRepository>();
        await repository.Criar(agenda);
        await _context.Commit();

        // Act
        var result = await repository.BuscarPorMedicoHora(agenda.MedicoId, agenda.DataHora);

        // Assert
        result.Should().BeEquivalentTo(agenda);
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
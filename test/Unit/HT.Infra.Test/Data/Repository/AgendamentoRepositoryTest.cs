﻿using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using HT.Test.Utils.Fixtures;
using Microsoft.EntityFrameworkCore;
using HT.Infra.Data;
using HT.Domain.Repository;
using HT.Infra.Data.Repository;
using HT.Domain.Models;

namespace HT.Infra.Test.Data.Repository;

public class AgendamentoRepositoryTest : IDisposable
{
    private readonly HackatonDbContext _context;
    private readonly IFixture _fixture;
    private bool disposed = false;

    public AgendamentoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<HackatonDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDbAgendamento")
            .Options;

        _context = new HackatonDbContext(options);

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customize(new CpfCustomization());
        _fixture.Register<IAgendamentoRepository>(() => new AgendamentoRepository(_context));
    }

    [Fact]
    public async Task DeveCriarUmaAgendamento()
    {
        // Arrange
        var agendamento = _fixture.Create<Agendamento>();
        var repository = _fixture.Create<IAgendamentoRepository>();

        // Act
        await repository.Criar(agendamento);
        bool commit = await _context.Commit();

        // Assert
        commit.Should().BeTrue();
        _context.Agendamentos.Should().Contain(agendamento);
        var medicoSalvo = await _context.Agendamentos.FindAsync(agendamento.Id);
        medicoSalvo.Should().NotBeNull();
        medicoSalvo.Should().BeEquivalentTo(agendamento);
        repository.UnitOfWork.Should().Be(_context);
    }

    [Fact]
    public async Task DeveBuscarAgendamentoPorAgenda()
    {
        // Arrange
        var agendamento = _fixture.Create<Agendamento>();
        var repository = _fixture.Create<IAgendamentoRepository>();
        await repository.Criar(agendamento);
        await _context.Commit();

        // Act
        var result = await repository.BuscarPorAgenda(agendamento.AgendaId);

        // Assert
        result.Should().BeEquivalentTo(agendamento);
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
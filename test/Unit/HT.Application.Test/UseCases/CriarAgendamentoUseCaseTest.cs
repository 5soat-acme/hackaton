using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using System.Globalization;
using HT.Domain.Repository;
using HT.Core.Commons.Email;
using HT.Domain.Models;
using FluentAssertions;
using HT.Test.Utils.Fixtures;
using HT.Application.UseCases.Interfaces;
using HT.Application.UseCases;
using HT.Application.DTOs.Requests;

namespace HT.Application.Test.UseCases;

public class CriarAgendamentoUseCaseTest
{
    private readonly IFixture _fixture;

    public CriarAgendamentoUseCaseTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        var agendamentoRepositoryMock = _fixture.Freeze<Mock<IAgendamentoRepository>>();
        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        var pacienteRepositoryMock = _fixture.Freeze<Mock<IPacienteRepository>>();
        var emailSenderMock = _fixture.Freeze<Mock<IEmailSender>>();
        _fixture.Customize(new CpfCustomization());
        _fixture.Register<ICriarAgendamentoUseCase>(() => new CriarAgendamentoUseCase(agendamentoRepositoryMock.Object,
            agendaRepositoryMock.Object, medicoRepositoryMock.Object, pacienteRepositoryMock.Object, emailSenderMock.Object));
    }

    [Fact]
    public async Task DeveCriarAgendamento()
    {
        // Arrange
        var agendamentoRepositoryMock = _fixture.Freeze<Mock<IAgendamentoRepository>>();
        var agendaRepositoryMock = _fixture.Freeze<Mock<IAgendaRepository>>();
        var medicoRepositoryMock = _fixture.Freeze<Mock<IMedicoRepository>>();
        var pacienteRepositoryMock = _fixture.Freeze<Mock<IPacienteRepository>>();
        var emailSenderMock = _fixture.Freeze<Mock<IEmailSender>>();

        string dataString = "21/09/2024 20:00:00";
        DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var medico = _fixture.Create<Medico>();
        var agenda = new Agenda(medico.Id, data);
        var paciente = _fixture.Create<Paciente>();
        var agendamentoDto = new CriarAgendamentoDto
        {
            AgendaId = agenda.Id,
            PacienteId = paciente.Id,
        };

        agendamentoRepositoryMock.Setup(x => x.Criar(It.IsAny<Agendamento>())).Returns(Task.CompletedTask);
        agendamentoRepositoryMock.Setup(x => x.BuscarPorAgenda(agendamentoDto.AgendaId)).ReturnsAsync((Agendamento?)null);
        agendaRepositoryMock.Setup(x => x.BuscarPorId(agenda.Id)).ReturnsAsync(agenda);
        medicoRepositoryMock.Setup(x => x.BuscarPorId(medico.Id)).ReturnsAsync(medico);
        pacienteRepositoryMock.Setup(x => x.BuscarPorId(paciente.Id)).ReturnsAsync(paciente);
        emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true)).Returns(Task.CompletedTask);

        var useCase = _fixture.Create<ICriarAgendamentoUseCase>();

        // Act
        var resultado = await useCase.Handle(agendamentoDto);

        // Assert
        agendamentoRepositoryMock.Verify(x => x.Criar(It.IsAny<Agendamento>()), Times.Once);
        agendaRepositoryMock.Verify(x => x.BuscarPorId(It.IsAny<Guid>()), Times.Once);
        medicoRepositoryMock.Verify(x => x.BuscarPorId(It.IsAny<Guid>()), Times.Once);
        pacienteRepositoryMock.Verify(x => x.BuscarPorId(It.IsAny<Guid>()), Times.Once);
        agendamentoRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
        resultado.IsValid.Should().BeTrue();
        resultado.ValidationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeveRetornarErro_QuandoCriarAgendamentoJaExistente()
    {
        // Arrange
        var agendamentoRepositoryMock = _fixture.Freeze<Mock<IAgendamentoRepository>>();
        var agendamentoDto = _fixture.Create<CriarAgendamentoDto>();
        var agendamento = _fixture.Create<Agendamento>();
        agendamentoRepositoryMock.Setup(x => x.BuscarPorAgenda(agendamentoDto.AgendaId)).ReturnsAsync(agendamento);

        var useCase = _fixture.Create<ICriarAgendamentoUseCase>();

        // Act
        var resultado = await useCase.Handle(agendamentoDto);

        // Assert
        agendamentoRepositoryMock.Verify(x => x.Criar(It.IsAny<Agendamento>()), Times.Never);
        agendamentoRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Never);
        resultado.IsValid.Should().BeFalse();
        resultado.GetErrorMessages().Count(x => x == "Já existe agendamento efetuado para essa data/hora.").Should().Be(1);
    }
}

using HT.Agendas.Application.DTOs.Requests;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Domain.Models;
using HT.Agendas.Domain.Repository;
using HT.Cadastros.Domain.Repository;
using HT.Core.Commons.Communication;
using HT.Core.Commons.Email;
using HT.Core.Commons.UseCases;
using Microsoft.Extensions.Logging;

namespace HT.Agendas.Application.UseCases;

public class CriarAgendamentoUseCase : CommonUseCase, ICriarAgendamentoUseCase
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IAgendaRepository _agendaRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CriarAgendamentoUseCase> _logger;

    public CriarAgendamentoUseCase(IAgendamentoRepository agendamentoRepository,
        IAgendaRepository agendaRepository,
        IMedicoRepository medicoRepository,
        IPacienteRepository pacienteRepository,
        IEmailSender emailSender,
        ILogger<CriarAgendamentoUseCase> logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _agendaRepository = agendaRepository;
        _medicoRepository = medicoRepository;
        _pacienteRepository = pacienteRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<OperationResult<Guid>> Handle(CriarAgendamentoDto dto)
    {
        if (!await ValidarAgendamentoExistente(dto)) return OperationResult<Guid>.Failure(ValidationResult);

        var agendamento = new Agendamento(dto.AgendaId, dto.PacienteId);
        await _agendamentoRepository.Criar(agendamento);
        await PersistData(_agendamentoRepository.UnitOfWork);
        _logger.LogInformation("TESTE DE LOG");
        _logger.LogInformation("TESTE DE LOG");
        _logger.LogInformation("TESTE DE LOG");
        await EnviarEmail(dto);
        return OperationResult<Guid>.Success(agendamento.Id);
    }

    private async Task<bool> ValidarAgendamentoExistente(CriarAgendamentoDto dto)
    {
        var result = await _agendamentoRepository.BuscarPorAgenda(dto.AgendaId);

        if (result is not null)
        {
            AddError("Já existe agendamento efetuado para essa data/hora.");
            return false;
        }

        return true;
    }

    private async Task EnviarEmail(CriarAgendamentoDto dto)
    {
        var agenda = await _agendaRepository.BuscarPorId(dto.AgendaId);
        var medico = await _medicoRepository.BuscarPorId(agenda!.MedicoId);
        var paciente = await _pacienteRepository.BuscarPorId(dto.PacienteId);

        var body = MontarCorpoEmail(medico!.Nome, paciente!.Nome, agenda.DataHora);
        await _emailSender.SendEmailAsync(medico!.Email, "Health&Med - Nova consulta agendada”", body, true);
    }

    private string MontarCorpoEmail(string nomeMedico, string nomePaciente, DateTime dataHora)
    {
        var dataFormatada = dataHora.ToString("dd/MM/yyyy");
        var horaFormatada = dataHora.ToString("HH:mm");

        return @$"
                    <!DOCTYPE html>
                    <html lang=""pt-BR"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Nova Consulta Agendada</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 20px auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                            }}
                            .email-header {{
                                background-color: #2c3e50;
                                color: #ffffff;
                                padding: 10px;
                                border-radius: 8px 8px 0 0;
                                text-align: center;
                            }}
                            .email-body {{
                                padding: 20px;
                                color: #333333;
                                line-height: 1.6;
                            }}
                            .email-body h1 {{
                                font-size: 24px;
                                color: #333333;
                                margin: 0 0 20px;
                            }}
                            .email-body p {{
                                font-size: 16px;
                                margin: 0 0 10px;
                            }}
                            .email-footer {{
                                margin-top: 20px;
                                text-align: center;
                                color: #999999;
                                font-size: 12px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""email-container"">
                            <div class=""email-header"">
                                <h2>Health&Med - Nova consulta agendada</h2>
                            </div>
                            <div class=""email-body"">
                                <h1>Olá, {nomeMedico}!</h1>
                                <p>Você tem uma nova consulta marcada!</p>
                                <p><strong>Paciente:</strong> {nomePaciente}</p>
                                <p><strong>Data e horário:</strong> {dataFormatada} às {horaFormatada}</p>
                            </div>
                            <div class=""email-footer"">
                                <p>© 2024 Health&Med. Todos os direitos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>
                    ";
    }
}
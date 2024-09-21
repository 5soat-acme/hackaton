using HT.Core.Commons.Email;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;

namespace HT.Infra.Commons.Email;
public class EmailSender : IEmailSender
{
    private EmailSettings _emailSettings { get; }
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<EmailSettings> optionsAccessor,
        ILogger<EmailSender> logger)
    {
        _emailSettings = optionsAccessor.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string emailDestino, string assunto, string body, bool isBodyHtml)
    {
        await Execute(_emailSettings, emailDestino, assunto, body, isBodyHtml);
    }

    private async Task Execute(EmailSettings options, string emailDestino, string assunto, string body, bool isBodyHtml)
    {
        _logger.LogInformation("Host: " + options.Host);
        _logger.LogInformation("Port: " + options.Port);
        _logger.LogInformation("Password: " + options.Password);
        _logger.LogInformation("UserName: " + options.UserName);
        _logger.LogInformation("FromName: " + options.FromName);
        _logger.LogInformation("FromEmail: " + options.FromEmail);

        var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
            EnableSsl = _emailSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = assunto,
            Body = body,
            IsBodyHtml = isBodyHtml
        };

        mailMessage.To.Add(emailDestino);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
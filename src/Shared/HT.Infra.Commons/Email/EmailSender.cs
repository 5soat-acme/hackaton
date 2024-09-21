using HT.Core.Commons.Email;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace HT.Infra.Commons.Email;
public class EmailSender : IEmailSender
{
    private EmailSettings _emailSettings { get; }

    public EmailSender(IOptions<EmailSettings> optionsAccessor)
    {
        _emailSettings = optionsAccessor.Value;
    }

    public async Task SendEmailAsync(string emailDestino, string assunto, string body, bool isBodyHtml)
    {
        await Execute(emailDestino, assunto, body, isBodyHtml);
    }

    private async Task Execute(string emailDestino, string assunto, string body, bool isBodyHtml)
    {
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
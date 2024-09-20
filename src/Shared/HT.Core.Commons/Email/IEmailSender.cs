namespace HT.Core.Commons.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string emailDestino, string assunto, string body, bool isBodyHtml);
}
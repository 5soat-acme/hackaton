using HT.Core.Commons.Email;
using HT.Infra.Commons.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HT.WebApi.Commons.Email;

public static class EmailConfig
{
    public static void AddEmailConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        var settingsSection = configuration.GetSection("EmailSettings");
        services.Configure<EmailSettings>(settingsSection);
    }
}

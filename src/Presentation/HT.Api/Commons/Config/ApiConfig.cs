using System.Text.Json.Serialization;
using HT.Api.Commons.Extensions;
using HT.Api.Contexts.Medicos.Config;
using HT.Api.Contexts.Produtos.Config;
using HT.WebApi.Commons.Identity;
using HT.WebApi.Commons.Users;

namespace HT.Api.Commons.Config;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfig(env);

        services.AddEventBusConfig();

        services.RegisterServicesPacientes(configuration);
        services.RegisterServicesMedicos(configuration);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserApp, UserApp>();
        services.AddJwtConfiguration(configuration);

        return services;
    }

    public static WebApplication UseApiConfig(this WebApplication app)
    {
        app.UseSwaggerConfig();

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            await next();
        });

        app.MapControllers();

        app.UseMiddleware<ExceptionMiddleware>();

        app.SubscribeEventHandlers();

        return app;
    }
}
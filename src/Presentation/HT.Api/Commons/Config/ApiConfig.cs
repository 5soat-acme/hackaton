using System.Text.Json.Serialization;
using HT.Api.Commons.Extensions;

namespace HT.Api.Commons.Config;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfig(env);

        services.RegisterServices(configuration);

        services.AddIdentityConfig(configuration);

        return services;
    }

    public static WebApplication UseApiConfig(this WebApplication app)
    {
        app.RunMigrations();

        app.UseSwaggerConfig();

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            await next();
        });

        app.MapControllers();

        app.UseIdentityConfig();

        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
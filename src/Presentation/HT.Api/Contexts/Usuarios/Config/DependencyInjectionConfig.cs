using HT.Usuarios.Application.Services;
using HT.Usuarios.Application.Services.Interfaces;

namespace HT.Api.Contexts.Usuarios.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesUsuarios(this IServiceCollection services)
    {
        // Application - Use Cases
        services.AddScoped<IAcessoAppService, AcessoAppService>();

        return services;
    }
}
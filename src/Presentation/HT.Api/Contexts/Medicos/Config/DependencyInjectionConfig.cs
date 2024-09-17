using Microsoft.EntityFrameworkCore;
using HT.Medicos.Application.UseCases.Interfaces;
using HT.Medicos.Application.UseCases;
using HT.Medicos.Domain.Repository;
using HT.Medicos.Infra.Data.Repository;
using HT.Medicos.Infra.Data;

namespace HT.Api.Contexts.Medicos.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesMedicos(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Use Cases
        services.AddScoped<ICriarMedicoUseCase, CriarMedicoUseCase>();

        // Infra - Data
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        services.AddDbContext<MedicoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
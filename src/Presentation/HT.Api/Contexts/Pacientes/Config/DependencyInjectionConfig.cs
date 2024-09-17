using HT.Pacientes.Application.UseCases;
using HT.Pacientes.Application.UseCases.Interfaces;
using HT.Pacientes.Domain.Repository;
using HT.Pacientes.Infra.Data;
using HT.Pacientes.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace HT.Api.Contexts.Produtos.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesPacientes(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Use Cases
        services.AddScoped<ICriarPacienteUseCase, CriarPacienteUseCase>();

        // Infra - Data
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddDbContext<PacienteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
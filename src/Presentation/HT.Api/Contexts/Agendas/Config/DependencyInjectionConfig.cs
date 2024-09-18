using HT.Agendas.Domain.Repository;
using HT.Agendas.Infra.Data;
using HT.Agendas.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using HT.Agendas.Application.UseCases.Interfaces;
using HT.Agendas.Application.UseCases;

namespace HT.Api.Contexts.Agendas.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesAgendas(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Use Cases
        services.AddScoped<ICriarAgendaUseCase, CriarAgendaUseCase>();
        services.AddScoped<IRemoverAgendaUseCase, RemoverAgendaUseCase>();
        services.AddScoped<IConsultarAgendaUseCase, ConsultarAgendaUseCase>();
        services.AddScoped<ICriarAgendamentoUseCase, CriarAgendamentoUseCase>();

        // Infra - Data
        services.AddScoped<IAgendaRepository, AgendaRepository>();
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

        services.AddDbContext<AgendaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
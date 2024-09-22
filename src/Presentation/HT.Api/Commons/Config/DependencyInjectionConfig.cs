using HT.Application.UseCases.Interfaces;
using HT.Application.UseCases;
using HT.Domain.Repository;
using HT.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using HT.Infra.Data;
using HT.Application.Services;
using HT.Application.Services.Interfaces;

namespace HT.Api.Commons.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Use Cases
        services.AddScoped<ICriarPacienteUseCase, CriarPacienteUseCase>();
        services.AddScoped<ICriarMedicoUseCase, CriarMedicoUseCase>();
        services.AddScoped<IBuscarMedicosUseCase, BuscarMedicosUseCase>();
        services.AddScoped<IAcessoAppService, AcessoAppService>();
        services.AddScoped<ICriarAgendaUseCase, CriarAgendaUseCase>();
        services.AddScoped<IRemoverAgendaUseCase, RemoverAgendaUseCase>();
        services.AddScoped<IConsultarAgendaUseCase, ConsultarAgendaUseCase>();
        services.AddScoped<ICriarAgendamentoUseCase, CriarAgendamentoUseCase>();

        // Infra - Data
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        services.AddScoped<IAgendaRepository, AgendaRepository>();
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

        services.AddDbContext<HackatonDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
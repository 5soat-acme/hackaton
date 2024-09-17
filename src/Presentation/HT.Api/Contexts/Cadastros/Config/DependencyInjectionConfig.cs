using HT.Cadastros.Application.UseCases;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Domain.Repository;
using HT.Cadastros.Infra.Data;
using HT.Cadastros.Infra.Data.Repository;
using HT.Usuarios.Application.Services;
using HT.Usuarios.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Api.Contexts.Cadastros.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesCadastros(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Use Cases
        services.AddScoped<ICriarPacienteUseCase, CriarPacienteUseCase>();
        services.AddScoped<ICriarMedicoUseCase, CriarMedicoUseCase>();
        services.AddScoped<IBuscarMedicosUseCase, BuscarMedicosUseCase>();
        services.AddScoped<IAcessoAppService, AcessoAppService>();

        // Infra - Data
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IMedicoRepository, MedicoRepository>();

        services.AddDbContext<CadastroDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
using HT.Agendas.Infra.Data;
using HT.Cadastros.Infra.Data;
using HT.Usuarios.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HT.Api.Commons.Config;


public static class MigrationsConfig
{
    public static WebApplication RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var cadastro = scope.ServiceProvider.GetRequiredService<CadastroDbContext>();
        cadastro.Database.Migrate();

        var usuario = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
        usuario.Database.Migrate();

        var agenda = scope.ServiceProvider.GetRequiredService<AgendaDbContext>();
        agenda.Database.Migrate();

        return app;
    }
}
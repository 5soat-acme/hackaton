using HT.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HT.Api.Commons.Config;

public static class MigrationsConfig
{
    public static WebApplication RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var cadastro = scope.ServiceProvider.GetRequiredService<HackatonDbContext>();
        cadastro.Database.Migrate();

        var usuario = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        usuario.Database.Migrate();

        return app;
    }
}
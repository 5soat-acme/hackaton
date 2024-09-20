using HT.Usuarios.Infra.Data;
using HT.Usuarios.Infra.Extensions;
using HT.WebApi.Commons.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HT.WebApi.Commons.Identity;
using HT.WebApi.Commons.Email;

namespace HT.Api.Commons.Config;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserApp, UserApp>();

        services.AddDbContext<UsuarioDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<TraducaoPortugues>()
            .AddEntityFrameworkStores<UsuarioDbContext>()
            .AddDefaultTokenProviders();

        services.AddJwtConfiguration(configuration);
        services.AddEmailConfiguration(configuration);

        return services;
    }

    public static WebApplication UseIdentityConfig(this WebApplication app)
    {
        app.UseAuthConfiguration();
        return app;
    }
}
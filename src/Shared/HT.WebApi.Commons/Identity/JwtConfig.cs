using System.Text;
using HT.Core.Commons.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HT.WebApi.Commons.Identity;

public static class JwtConfig
{
    public static void AddJwtConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache()
            .AddDataProtection();

        var settingsSection = configuration.GetSection("IdentidadeSettings");
        services.Configure<IdentitySettings>(settingsSection);
        var identitySettings = settingsSection.Get<IdentitySettings>();
        var key = Encoding.ASCII.GetBytes(identitySettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.RequireHttpsMetadata = false;
            bearerOptions.SaveToken = true;
            bearerOptions.Authority = identitySettings.Issuer;
            bearerOptions.Audience = identitySettings.ValidIn;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = identitySettings.ValidIn,
                ValidIssuer = identitySettings.Issuer,
                ValidateLifetime = true
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AccessPolicy.MEDICOPOLICY, policy => policy.RequireRole(TipoAcesso.MEDICO.ToString()));
            options.AddPolicy(AccessPolicy.PACIENTEPOLICY, policy => policy.RequireRole(TipoAcesso.PACIENTE.ToString()));
        });
    }

    public static void UseAuthConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
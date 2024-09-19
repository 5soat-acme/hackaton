using HT.Core.Commons.Communication;
using HT.Core.Commons.Extensions;
using HT.Core.Commons.Identity;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Usuarios.Application.DTOs.Responses;
using HT.Usuarios.Application.Services.Interfaces;
using HT.Usuarios.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HT.Usuarios.Application.Services;

public class AcessoAppService : IAcessoAppService
{
    private readonly IdentitySettings _identitySettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AcessoAppService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<IdentitySettings> settings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _identitySettings = settings.Value;
    }

    public async Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(
        NovoUsuario novoUsuario)
    {
        var newApplicationUser = new ApplicationUser
        {
            UserName = novoUsuario.Email,
            Email = novoUsuario.Email,
            EmailConfirmed = true,
            TipoAcesso = novoUsuario.TipoAcesso.ToString(),
            Cpf = novoUsuario.Cpf,
            CorrelacaoId = novoUsuario.Id
        };

        await CriarRole(novoUsuario.TipoAcesso.ToString());
        var identityResult = await _userManager.CreateAsync(newApplicationUser, novoUsuario.Senha);
        if (identityResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(newApplicationUser, novoUsuario.TipoAcesso.ToString());
        }

        var errors = new List<string>();
        foreach (var error in identityResult.Errors)
            errors.Add(error.Description);

        return OperationResult<RespostaTokenAcesso>.Failure(errors);
    }

    private async Task CriarRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    public async Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso usuario)
    {
        var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
        }

        var applicationUser = await _userManager.FindByEmailAsync(usuario.Email);


        return await Identificar(applicationUser);
    }

    private async Task<OperationResult<RespostaTokenAcesso>> Identificar(ApplicationUser? applicationUser)
    {
        if (applicationUser is not null)
            return OperationResult<RespostaTokenAcesso>.Success(await GerarTokenUsuarioIdentificado(applicationUser));

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
    }

    private async Task<RespostaTokenAcesso> GerarTokenUsuarioIdentificado(ApplicationUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var identityClaims = await ObterClaimsUsuario(claims, user);
        var encodedToken = CodificarToken(identityClaims);

        return ObterRespostaToken(encodedToken, claims, user);
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims,
        ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.CorrelacaoId.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf,
            DateTime.UtcNow.ToUnixEpochDate().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
            DateTime.UtcNow.ToUnixEpochDate().ToString(),
            ClaimValueTypes.Integer64));
        claims.Add(new Claim("user_type", "registred"));
        claims.Add(new Claim("session_id", Guid.NewGuid().ToString()));

        foreach (var userRole in userRoles) claims.Add(new Claim("role", userRole));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_identitySettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _identitySettings.Issuer,
            Audience = _identitySettings.ValidIn,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_identitySettings.ExpirationHours),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private RespostaTokenAcesso ObterRespostaToken(string encodedToken,
        IEnumerable<Claim> claims, ApplicationUser? user = null)
    {
        return new RespostaTokenAcesso
        {
            Token = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_identitySettings.ExpirationHours).TotalSeconds,
            User = new UsuarioToken
            {
                Id = user?.Id,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }
}
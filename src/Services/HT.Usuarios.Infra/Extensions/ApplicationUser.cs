using Microsoft.AspNetCore.Identity;

namespace HT.Usuarios.Infra.Extensions;

public class ApplicationUser : IdentityUser
{
    public string TipoAcesso { get; set; }
    public Guid CorrelacaoId { get; set; }
}
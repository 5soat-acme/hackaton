using Microsoft.AspNetCore.Identity;

namespace HT.Infra.Extensions;

public class ApplicationUser : IdentityUser
{
    public string TipoAcesso { get; set; }
    public string Cpf { get; set; }
    public Guid CorrelacaoId { get; set; }
}
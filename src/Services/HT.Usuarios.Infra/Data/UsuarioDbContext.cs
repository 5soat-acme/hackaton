using HT.Usuarios.Infra.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HT.Usuarios.Infra.Data;

public class UsuarioDbContext : IdentityDbContext<ApplicationUser>
{
    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options)
    {
    }
}
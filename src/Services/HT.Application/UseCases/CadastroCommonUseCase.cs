using HT.Core.Commons.UseCases;
using HT.Core.Commons.Utils;
using HT.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HT.Application.UseCases;

public abstract class CadastroCommonUseCase : CommonUseCase
{
    protected readonly UserManager<ApplicationUser> _userManager;

    protected CadastroCommonUseCase(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected async Task<bool> ValidarUsuario(string email, string cpf)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            AddError("Já existe cadastrado com esse email");
            return false;
        }

        user = await _userManager.Users.FirstOrDefaultAsync(x => x.Cpf == cpf.SomenteNumeros());
        if (user is not null)
        {
            AddError("Já existe cadastrado com esse CPF");
            return false;
        }

        return true;
    }
}
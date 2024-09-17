using System.ComponentModel.DataAnnotations;

namespace HT.Usuarios.Application.DTOs.Requests;

public class UsuarioAcesso
{
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }

    public string Senha { get; set; }
}
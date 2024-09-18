using HT.Core.Commons.Identity;
using System.ComponentModel.DataAnnotations;

namespace HT.Usuarios.Application.DTOs.Requests;

public class NovoUsuario
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Senha { get; set; }

    public TipoAcesso TipoAcesso { get; set; }
    public Guid Id { get; set; }
}
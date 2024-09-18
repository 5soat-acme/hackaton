namespace HT.Usuarios.Application.DTOs.Responses;

public class UsuarioToken
{
    public string? Id { get; set; }
    public IEnumerable<UsuarioClaim> Claims { get; set; }
}
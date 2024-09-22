namespace HT.Application.DTOs.Requests;

public class CriarPacienteDto
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string ConfirmacaoSenha { get; set; }
}
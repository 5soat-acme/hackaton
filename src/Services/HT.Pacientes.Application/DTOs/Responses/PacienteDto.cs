namespace HT.Pacientes.Application.DTOs.Responses;

public class PacienteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
}
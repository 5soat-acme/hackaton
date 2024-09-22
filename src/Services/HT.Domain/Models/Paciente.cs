using HT.Core.Commons.DomainObjects;
using HT.Core.Commons.ValueObjects;

namespace HT.Domain.Models;

public class Paciente : Entity, IAggregateRoot
{
    private Paciente() { }

    public Paciente(string nome, Cpf cpf, string email)
    {
        ValidarPaciente(nome, cpf, email);

        Nome = nome;
        Cpf = cpf;
        Email = email;
    }

    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string Email { get; private set; }

    public void ValidarPaciente(string nome, Cpf cpf, string email)
    {
        ValidarNome(nome);
        ValidarCPF(cpf);
        ValidarEmail(email);
    }

    public void ValidarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome)) throw new DomainException("Nome inválido");
    }

    public void ValidarCPF(Cpf cpf)
    {
        if (cpf == null) throw new DomainException("CPF inválido");
    }

    public void ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new DomainException("Email inválido");
    }
}
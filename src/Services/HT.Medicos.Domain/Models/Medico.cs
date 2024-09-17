using HT.Core.Commons.DomainObjects;
using HT.Core.Commons.ValueObjects;

namespace HT.Medicos.Domain.Models;

public class Medico : Entity, IAggregateRoot
{
    private Medico() { }

    public Medico(string nome, Cpf cpf, string nroCrm, string email, string senha)
    {
        ValidarMedico(nome, cpf, nroCrm, email, senha);

        Nome = nome;
        Cpf = cpf;
        NroCrm = nroCrm;
        Email = email;
        Senha = senha;
    }

    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string NroCrm { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }

    public void ValidarMedico(string nome, Cpf cpf, string nroCrm, string email, string senha)
    {
        ValidarNome(nome);
        ValidarCPF(cpf);
        ValidarNroCrm(nroCrm);
        ValidarEmail(email);
        ValidarSenha(senha);
    }

    public void ValidarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome)) throw new DomainException("Nome inválido");
    }

    public void ValidarCPF(Cpf cpf)
    {
        if (cpf == null) throw new DomainException("CPF inválido");
    }

    public void ValidarNroCrm(string nroCrm)
    {
        if (string.IsNullOrEmpty(nroCrm)) throw new DomainException("Número CRM inválido");
    }

    public void ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new DomainException("Email inválido");
    }

    public void ValidarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha)) throw new DomainException("Senha inválida");
    }
}
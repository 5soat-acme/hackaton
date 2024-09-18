using HT.Core.Commons.DomainObjects;
using HT.Core.Commons.ValueObjects;

namespace HT.Cadastros.Domain.Models;

public class Medico : Entity, IAggregateRoot
{
    private Medico() { }

    public Medico(string nome, Cpf cpf, string nroCrm, string email)
    {
        ValidarMedico(nome, cpf, nroCrm, email);

        Nome = nome;
        Cpf = cpf;
        NroCrm = nroCrm;
        Email = email;
    }

    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public string NroCrm { get; private set; }
    public string Email { get; private set; }

    public void ValidarMedico(string nome, Cpf cpf, string nroCrm, string email)
    {
        ValidarNome(nome);
        ValidarCPF(cpf);
        ValidarNroCrm(nroCrm);
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

    public void ValidarNroCrm(string nroCrm)
    {
        if (string.IsNullOrEmpty(nroCrm)) throw new DomainException("Número CRM inválido");
    }

    public void ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new DomainException("Email inválido");
    }
}
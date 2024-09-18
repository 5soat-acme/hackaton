using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;

namespace HT.Cadastros.Domain.Repository;

public interface IMedicoRepository : IRepository<Medico>
{
    Task Criar(Medico medico);
    Task<IEnumerable<Medico>> Buscar();
    Task<Medico?> BuscarPorEmail(string email);
    Task<Medico?> BuscarPorCpf(string cpf);
}
using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;

namespace HT.Cadastros.Domain.Repository;

public interface IMedicoRepository : IRepository<Medico>
{
    void Criar(Medico medico);
    Task<IEnumerable<Medico>> Buscar();
}
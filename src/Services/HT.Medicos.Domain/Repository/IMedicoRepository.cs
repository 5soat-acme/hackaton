using HT.Core.Commons.Repository;
using HT.Medicos.Domain.Models;

namespace HT.Medicos.Domain.Repository;

public interface IMedicoRepository : IRepository<Medico>
{
    void Criar(Medico medico);
    Task<IEnumerable<Medico>> Buscar();
}
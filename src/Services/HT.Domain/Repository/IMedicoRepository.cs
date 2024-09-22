using HT.Core.Commons.Repository;
using HT.Domain.Models;

namespace HT.Domain.Repository;

public interface IMedicoRepository : IRepository<Medico>
{
    Task Criar(Medico medico);
    Task<IEnumerable<Medico>> Buscar();
    Task<Medico?> BuscarPorId(Guid medicoId);
}
using HT.Core.Commons.Repository;
using HT.Domain.Models;

namespace HT.Domain.Repository;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task Criar(Paciente paciente);
    Task<IEnumerable<Paciente>> Buscar();
    Task<Paciente?> BuscarPorId(Guid pacienteId);
}
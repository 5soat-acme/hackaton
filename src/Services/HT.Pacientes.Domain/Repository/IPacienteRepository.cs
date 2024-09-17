using HT.Core.Commons.Repository;
using HT.Pacientes.Domain.Models;

namespace HT.Pacientes.Domain.Repository;

public interface IPacienteRepository : IRepository<Paciente>
{
    void Criar(Paciente paciente);
    Task<IEnumerable<Paciente>> Buscar();
}
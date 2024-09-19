using HT.Core.Commons.Repository;
using HT.Cadastros.Domain.Models;

namespace HT.Cadastros.Domain.Repository;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task Criar(Paciente paciente);
    Task<IEnumerable<Paciente>> Buscar();
}
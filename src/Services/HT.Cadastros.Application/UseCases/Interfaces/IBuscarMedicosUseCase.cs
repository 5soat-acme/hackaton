using HT.Cadastros.Application.DTOs.Responses;

namespace HT.Cadastros.Application.UseCases.Interfaces;

public interface IBuscarMedicosUseCase
{
    Task<IEnumerable<MedicoDto>> Buscar();
}

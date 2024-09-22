using HT.Application.DTOs.Responses;

namespace HT.Application.UseCases.Interfaces;

public interface IBuscarMedicosUseCase
{
    Task<IEnumerable<MedicoDto>> Buscar();
}

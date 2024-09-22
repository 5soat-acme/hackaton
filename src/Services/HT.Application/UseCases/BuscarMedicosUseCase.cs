using HT.Application.DTOs.Responses;
using HT.Application.Mappings;
using HT.Application.UseCases.Interfaces;
using HT.Domain.Repository;

namespace HT.Application.UseCases;

public class BuscarMedicosUseCase : IBuscarMedicosUseCase
{
    private readonly IMedicoRepository _medicoRepository;

    public BuscarMedicosUseCase(IMedicoRepository medicoRepository)
    {
        _medicoRepository = medicoRepository;
    }

    public async Task<IEnumerable<MedicoDto>> Buscar()
    {
        var medicos = await _medicoRepository.Buscar();
        return MedicoDomainToDtoMapper.Map(medicos);
    }
}
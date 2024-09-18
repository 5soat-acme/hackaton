using HT.Cadastros.Application.DTOs.Responses;
using HT.Cadastros.Application.Mappings;
using HT.Cadastros.Application.UseCases.Interfaces;
using HT.Cadastros.Domain.Repository;

namespace HT.Cadastros.Application.UseCases;

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
        return DomainToDtoMapper.Map(medicos);
    }
}
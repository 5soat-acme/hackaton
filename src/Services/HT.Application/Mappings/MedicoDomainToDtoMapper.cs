using HT.Application.DTOs.Responses;
using HT.Domain.Models;

namespace HT.Application.Mappings;

public static class MedicoDomainToDtoMapper
{
    public static MedicoDto Map(Medico medico)
    {
        return new MedicoDto
        {
            Id = medico.Id,
            Nome = medico.Nome,
            Email = medico.Email
        };
    }

    public static IEnumerable<MedicoDto> Map(IEnumerable<Medico> medicos)
    {
        return medicos.Select(Map).ToList();
    }
}
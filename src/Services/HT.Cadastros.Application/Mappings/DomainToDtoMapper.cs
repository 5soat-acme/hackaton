using HT.Cadastros.Application.DTOs.Responses;
using HT.Cadastros.Domain.Models;

namespace HT.Cadastros.Application.Mappings;

public static class DomainToDtoMapper
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
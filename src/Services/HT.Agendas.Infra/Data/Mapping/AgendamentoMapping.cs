using HT.Agendas.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HT.Cadastros.Domain.Models;

namespace HT.Agendas.Infra.Data.Mapping;

public class AgendamentoMapping : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("Agendamentos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.AgendaId)
            .IsRequired();

        builder.Property(p => p.PacienteId)
            .IsRequired();

        builder.HasIndex(p => p.AgendaId)
            .IsUnique();
    }
}
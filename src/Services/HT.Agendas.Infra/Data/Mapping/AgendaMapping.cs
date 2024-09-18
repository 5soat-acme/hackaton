using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HT.Agendas.Domain.Models;
using HT.Cadastros.Domain.Models;

namespace HT.Agendas.Infra.Data.Mapping;

public class AgendaMapping : IEntityTypeConfiguration<Agenda>
{
    public void Configure(EntityTypeBuilder<Agenda> builder)
    {
        builder.ToTable("Agendas");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.MedicoId)
            .IsRequired();

        builder.Property(c => c.DataHora)
            .IsRequired();

        builder.HasIndex(p => new { p.MedicoId, p.DataHora })
            .IsUnique();
    }
}
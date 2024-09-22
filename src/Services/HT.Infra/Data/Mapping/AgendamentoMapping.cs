using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HT.Domain.Models;

namespace HT.Infra.Data.Mapping;

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

        builder.HasOne<Agenda>()
            .WithMany()
            .HasForeignKey(p => p.AgendaId);

        builder.HasOne<Paciente>()
            .WithMany()
            .HasForeignKey(p => p.PacienteId);

        builder.HasIndex(p => p.AgendaId)
            .IsUnique();
    }
}
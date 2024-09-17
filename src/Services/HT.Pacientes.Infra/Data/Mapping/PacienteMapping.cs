using HT.Core.Commons.ValueObjects;
using HT.Pacientes.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HT.Pacientes.Infra.Data.Mapping;

public class PacienteMapping : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Numero)
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.CpfMaxLength})");
        });

        builder.Property(p => p.Email)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(p => p.Senha)
            .IsRequired()
            .HasColumnType("varchar(20)");
    }
}
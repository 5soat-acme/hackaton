using HT.Core.Commons.ValueObjects;
using HT.Medicos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HT.Medicos.Infra.Data.Mapping;

public class MedicoMapping : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        builder.ToTable("Medicos");

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

        builder.Property(p => p.NroCrm)
           .IsRequired()
           .HasColumnType("varchar(15)");

        builder.Property(p => p.Email)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(p => p.Senha)
            .IsRequired()
            .HasColumnType("varchar(20)");
    }
}
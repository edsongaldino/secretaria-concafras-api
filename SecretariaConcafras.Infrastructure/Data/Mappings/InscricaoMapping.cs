using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoMapping : IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("inscricoes");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.EventoId, x.ParticipanteId }).IsUnique();

        builder.Property(x => x.ResponsavelFinanceiroId)
               .HasColumnName("responsavel_financeiro_id")
               .IsRequired();

        // 1:1 Trabalhador (se usar)
        builder.HasOne(x => x.InscricaoTrabalhador)
         .WithOne(x => x.Inscricao)
         .HasForeignKey<InscricaoTrabalhador>(x => x.InscricaoId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.EventoId, x.ResponsavelFinanceiroId })
               .HasDatabaseName("ix_inscricoes_evento_responsavel");


    }
}

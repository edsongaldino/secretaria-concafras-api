using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoMapping : IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("inscricoes");
        builder.HasKey(x => x.Id).HasName("pk_inscricoes");

        builder.Property(x => x.EventoId).HasColumnName("evento_id").IsRequired();
        builder.Property(x => x.ParticipanteId).HasColumnName("participante_id").IsRequired();

        builder.HasOne(i => i.Evento)
            .WithMany(e => e.Inscricoes)
            .HasForeignKey(i => i.EventoId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_eventos_evento_id");

        builder.HasOne(i => i.Participante)
            .WithMany(p => p.Inscricoes)
            .HasForeignKey(i => i.ParticipanteId)
            .HasPrincipalKey(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_participantes_participante_id");

        // 1:1 opcionais (sem relação direta com CT!)
        builder.HasOne(i => i.InscricaoTrabalhador)
            .WithOne(it => it.Inscricao)
            .HasForeignKey<InscricaoTrabalhador>(it => it.InscricaoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.InscricaoCurso)
            .WithOne(ic => ic.Inscricao)
            .HasForeignKey<InscricaoCurso>(ic => ic.InscricaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

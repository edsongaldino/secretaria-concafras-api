using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoTrabalhadorMapping : IEntityTypeConfiguration<InscricaoTrabalhador>
{
    public void Configure(EntityTypeBuilder<InscricaoTrabalhador> b)
    {
        b.ToTable("inscricoes_trabalhador");
        b.HasKey(x => x.Id).HasName("pk_inscricoes_trabalhador");

        b.Property(x => x.InscricaoId).HasColumnName("inscricao_id").IsRequired();
        b.Property(x => x.ComissaoEventoId).HasColumnName("comissao_evento_id").IsRequired();
        b.Property(x => x.Nivel).HasColumnName("nivel").IsRequired();

        b.HasOne(x => x.Inscricao)
            .WithOne(i => i.InscricaoTrabalhador)
            .HasForeignKey<InscricaoTrabalhador>(x => x.InscricaoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_trabalhador_inscricoes_inscricao_id");

        b.HasOne(x => x.ComissaoEvento)
            .WithMany(ce => ce.InscricoesTrabalhadores)
            .HasForeignKey(x => x.ComissaoEventoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_trabalhador_comissoes_evento_comissao_evento_id");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoCursoMapping : IEntityTypeConfiguration<InscricaoCurso>
{
    public void Configure(EntityTypeBuilder<InscricaoCurso> builder)
    {
        builder.ToTable("inscricoes_curso");
        builder.HasKey(x => x.Id).HasName("pk_inscricoes_curso");

        builder.Property(x => x.InscricaoId).HasColumnName("inscricao_id").IsRequired();
        builder.Property(x => x.CursoId).HasColumnName("curso_id").IsRequired();

        builder.HasOne(x => x.Inscricao)
            .WithOne(i => i.InscricaoCurso)
            .HasForeignKey<InscricaoCurso>(x => x.InscricaoId)
            .HasPrincipalKey<Inscricao>(i => i.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_curso_inscricoes_inscricao_id");

        builder.HasOne(x => x.Curso)
            .WithMany(c => c.InscricoesCurso)
            .HasForeignKey(x => x.CursoId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscricoes_curso_cursos_curso_id");
    }
}
